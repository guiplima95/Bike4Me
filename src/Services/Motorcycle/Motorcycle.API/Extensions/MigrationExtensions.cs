using Microsoft.EntityFrameworkCore;
using Motorcycle.API.Infrastructure.Database;

namespace Motorcycle.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using MotorcycleContext context =
            scope.ServiceProvider.GetRequiredService<MotorcycleContext>();

        context.Database.Migrate();
    }
}
