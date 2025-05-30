﻿using Bike4Me.Application.Abstractions.Messaging;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Infrastructure.EventBus.Converters;
using Bike4Me.Infrastructure.EventBus.Interfaces;
using Bike4Me.Infrastructure.Logging;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bike4Me.Infrastructure.EventBus;

public class ApplicationEventBusRabbitMQ : IApplicationEventBus, IDisposable
{
    private readonly IApplicationRabbitMQPersistentConnection _persistentConnection;
    private readonly ILogger<ApplicationEventBusRabbitMQ> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _queueName;
    private readonly int _retryCount;
    private IModel? _consumerChannel;

    private readonly JsonSerializerOptions _jsonOptions;

    public ApplicationEventBusRabbitMQ(
        IApplicationRabbitMQPersistentConnection persistentConnection,
        IConfiguration configuration,
        ILogger<ApplicationEventBusRabbitMQ> logger,
        IServiceProvider serviceProvider)
    {
        _persistentConnection = persistentConnection;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _queueName = configuration["EventBus:RabbitMQ:ApplicationMessageQueue"] ?? throw new ArgumentNullException(nameof(configuration));
        _retryCount = configuration.GetValue("EventBus:EventBusRetryCount", 5);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        _jsonOptions.Converters.Add(new MessageConverter(typeof(Message).Assembly));
    }

    public Task PublishMessage<T>(T message) where T : Message
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        RetryPolicy policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(message: $"Could not publish event: {message.Id} after {time.TotalSeconds:n1}s", ex);
            });

        using IModel channel = _persistentConnection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        string jsonMessage = JsonSerializer.Serialize((Message)message, _jsonOptions);
        byte[] body = Encoding.UTF8.GetBytes(jsonMessage);

        policy.Execute(() =>
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: body);
        });

        return Task.CompletedTask;
    }

    public Task StartConsumer()
    {
        _consumerChannel = CreateConsumerChannel();
        StartBasicConsume();
        return Task.CompletedTask;
    }

    public Task StopConsumer()
    {
        if (_consumerChannel != null)
        {
            _consumerChannel.BasicCancel(_queueName);
            _consumerChannel.Close();
            _consumerChannel.Dispose();
            _consumerChannel = null;
        }
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_consumerChannel != null)
        {
            try
            {
                _consumerChannel.Close();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception while closing consumer channel");
            }
            finally
            {
                _consumerChannel.Dispose();
                _consumerChannel = null;
            }
        }
        GC.SuppressFinalize(this);
    }

    private IModel CreateConsumerChannel()
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var channel = _persistentConnection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        channel.CallbackException += (sender, e) =>
        {
            _logger.LogWarning<ApplicationEventBusRabbitMQ>("Recreating RabbitMQ consumer channel due to exception", e.Exception);

            _consumerChannel?.Dispose();
            _consumerChannel = CreateConsumerChannel();

            StartBasicConsume();
        };

        return channel;
    }

    private void StartBasicConsume()
    {
        if (_consumerChannel == null)
        {
            _logger.LogError("StartBasicConsume called but _consumerChannel is null");
            return;
        }

        var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
        consumer.Received += Consumer_Received;

        _consumerChannel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
    {
        string jsonData = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        object? message = null;

        try
        {
            message = JsonSerializer.Deserialize<Message>(jsonData, _jsonOptions);

            if (message is null)
            {
                _logger.LogError("Received null message after deserialization");
                _consumerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            switch (message)
            {
                case INotification notification:
                    await mediator.Publish(notification);
                    break;

                case IRequest request:
                    await mediator.Send(request);
                    break;

                default:
                    _logger.LogWarning("Received message does not implement INotification or IRequest");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing message: {message}");
        }
        finally
        {
            _consumerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
    }
}