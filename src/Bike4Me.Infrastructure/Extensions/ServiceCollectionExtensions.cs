using Azure.Storage.Blobs;
using Bike4Me.Application.Abstractions.Caching;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Abstractions.Storage;
using Bike4Me.Domain.Bikes;
using Bike4Me.Domain.Couriers;
using Bike4Me.Infrastructure.Caching;
using Bike4Me.Infrastructure.Database;
using Bike4Me.Infrastructure.EventBus;
using Bike4Me.Infrastructure.EventBus.Interfaces;
using Bike4Me.Infrastructure.NoSql;
using Bike4Me.Infrastructure.Repositories;
using Bike4Me.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using SharedKernel;

namespace Bike4Me.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddCache(services, configuration);
        AddHealthChecks(services, configuration);
        AddBlobStorage(services, configuration);
        AddRabbitMQ(services, configuration);
        AddMongoDb(services, configuration);
        AddRepositories(services);

        return services;
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddDbContext<Bike4MeContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
    }

    private static void AddCache(IServiceCollection services, IConfiguration configuration)
    {
        string redisConnectionString = configuration.GetConnectionString("Cache")!;

        Ensure.NotNullOrEmpty(redisConnectionString);

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        string redisConnectionString = configuration.GetConnectionString("Cache")!;

        Ensure.NotNullOrEmpty(connectionString);
        Ensure.NotNullOrEmpty(redisConnectionString);

        services.AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddRedis(redisConnectionString);
    }

    private static void AddBlobStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IBikeRepository, BikeRepository>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IBikeReportRepository, BikeReportRepository>();
        services.AddScoped<IBikeModelRepository, BikeModelRepository>();
    }

    private static void AddRabbitMQ(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<IConnectionFactory>(serviceProvider =>
        {
            return new ConnectionFactory()
            {
                Uri = new Uri(configuration["EventBus:RabbitMQ:Connection"]!),
                DispatchConsumersAsync = true,
            };
        });

        services
            .AddSingleton<IApplicationRabbitMQPersistentConnection, ApplicationRabbitMQPersistentConnection>()
            .AddSingleton<IApplicationEventBus, ApplicationEventBusRabbitMQ>();
    }

    private static void AddMongoDb(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var database = client.GetDatabase(settings.DatabaseName);

            return database.GetCollection<BikeReport>("bikes");
        });

        services.AddScoped(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<Bike>("bikes");
        });
    }
}