using Bike4Me.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bike4Me.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using Bike4MeContext context =
            scope.ServiceProvider.GetRequiredService<Bike4MeContext>();

        context.Database.Migrate();
    }
}