using Microsoft.EntityFrameworkCore;

namespace Stio.WorkflowManager.DemoApi.Data;

public static class MigrationExtension
{
    public static void EnsureDbExists(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        if (context == null || !context.Database.GetPendingMigrations()
                .Any())
        {
            return;
        }

        context.Database.Migrate();
    }
}