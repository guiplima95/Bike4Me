using Bike4Me.Application.Abstractions.Behaviors;
using Bike4Me.Application.Abstractions.Messaging;
using Bike4Me.Application.Abstractions.Messaging.Interfaces;
using Bike4Me.Application.Bikes.Queries;
using Bike4Me.Application.Bikes.Queries.Interfaces;
using Bike4Me.Application.Rentals.Queries;
using Bike4Me.Application.Rentals.Queries.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bike4Me.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddTransient<IMediatorHandler, MediatorHandler>();

        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly, includeInternalTypes: true);

        string connectionString = configuration["ConnectionStrings:DefaultConnection"] ??
            throw new InvalidOperationException("Could not found sql server connection string");

        services
            .AddHttpContextAccessor()
            .AddScoped<IBikesQueries>(_ => new BikesQueries(connectionString))
            .AddScoped<IRentalsQuery>(_ => new RentalsQuery(connectionString));

        return services;
    }
}