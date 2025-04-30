using Bike4Me.Application.Abstractions.Messaging.Interfaces;

namespace Bike4Me.API.Extensions;

public static class RabittMQConfigurationExtensions
{
    public static async Task<IApplicationBuilder> ConfigureEventBus(this IApplicationBuilder app)
    {
        IApplicationEventBus eventBus = app.ApplicationServices.GetRequiredService<IApplicationEventBus>();
        await eventBus.StartConsumer();

        return app;
    }
}