using Bike4Me.Infrastructure.EventBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System.Net.Sockets;
using Bike4Me.Infrastructure.Logging;
using Polly.Retry;

namespace Bike4Me.Infrastructure.EventBus;

public class ApplicationRabbitMQPersistentConnection(
    IConnectionFactory connectionFactory,
    ILogger<ApplicationRabbitMQPersistentConnection> logger,
    IConfiguration configuration) : IApplicationRabbitMQPersistentConnection
{
    private IConnection? _connection;
    private readonly int _retryCount = configuration.GetValue("EventBus:EventBusRetryCount", 5);
    private bool _disposed;

    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return _connection!.CreateModel();
    }

    public bool TryConnect()
    {
        logger.LogInformation<ApplicationRabbitMQPersistentConnection>("RabbitMQ Client is trying to connect");

        RetryPolicy policy = Policy.Handle<SocketException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                logger.LogWarning<ApplicationRabbitMQPersistentConnection>($"RabbitMQ Client could not connect after {time.TotalSeconds:n1}s", ex);
            }
        );

        policy.Execute(() =>
        {
            _connection = connectionFactory.CreateConnection();
        });

        if (IsConnected)
        {
            _connection!.ConnectionShutdown += OnConnectionShutdown!;
            _connection!.CallbackException += OnCallbackException!;
            _connection!.ConnectionBlocked += OnConnectionBlocked!;

            logger.LogInformation<ApplicationRabbitMQPersistentConnection>(
                $"RabbitMQ Client acquired a persistent connection to '{_connection.Endpoint.HostName}' and is subscribed to failure events");

            return true;
        }
        else
        {
            logger.LogCritical<ApplicationRabbitMQPersistentConnection>("FATAL ERROR: RabbitMQ connections could not be created and opened");

            return false;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            _connection!.Dispose();
        }
        catch (IOException ex)
        {
            logger.LogCritical<ApplicationRabbitMQPersistentConnection>(ex);
        }

        GC.SuppressFinalize(this);
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed)
        {
            return;
        }

        logger.LogWarning<ApplicationRabbitMQPersistentConnection>("A RabbitMQ connection is shutdown. Trying to re-connect...");

        TryConnect();
    }

    private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed)
        {
            return;
        }

        logger.LogWarning<ApplicationRabbitMQPersistentConnection>("A RabbitMQ connection is shutdown. Trying to re-connect...");

        TryConnect();
    }

    private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (_disposed)
        {
            return;
        }

        logger.LogWarning<ApplicationRabbitMQPersistentConnection>("A RabbitMQ connection is shutdown. Trying to re-connect...");

        TryConnect();
    }
}