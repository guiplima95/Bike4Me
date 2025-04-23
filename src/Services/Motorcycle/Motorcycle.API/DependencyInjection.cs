using Microsoft.EntityFrameworkCore;
using Motorcycle.API.Application.Abstractions.Caching;
using Motorcycle.API.Application.Queries;
using Motorcycle.API.Application.Queries.Interfaces;
using Motorcycle.API.Infrastructure.Caching;
using Motorcycle.API.Infrastructure.Database;
using Motorcycle.API.Infrastructure.Time;
using SharedKernel;
using System.Reflection;

namespace Motorcycle.API;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        AddDatabase(services, configuration);
        AddCaching(services, configuration);
        AddHealthChecks(services, configuration);

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            //config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection AddCustomDbContext(this IServiceCollection services, string connectionString)
    {
        return services
            .AddDbContext<MotorcycleContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    }).UseSnakeCaseNamingConvention();
            });
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Npgsql");
        Ensure.NotNullOrEmpty(connectionString);

        services
           .AddHttpContextAccessor()
           .AddScoped<IMotorcyclesQueries>(_ => new MotorcyclesQueries(connectionString));

        services.AddCustomDbContext(connectionString);
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string redisConnectionString = configuration.GetConnectionString("Cache")!;

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Npgsql")!)
            .AddRedis(configuration.GetConnectionString("Cache")!);
    }
}