using Bike4Me.Application.Motorcycles.Queries.Interfaces;
using Bike4Me.Application.Motorcycles.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FluentValidation;
using Bike4Me.Application.Abstractions.Behaviors;

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

        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly, includeInternalTypes: true);

        string connectionString = configuration["ConnectionStrings:DefaultConnection"] ??
            throw new InvalidOperationException("Could not found sql server connection string");

        return services
            .AddHttpContextAccessor()
            .AddScoped<IMotorcyclesQueries>(_ => new MotorcyclesQueries(connectionString));
    }
}