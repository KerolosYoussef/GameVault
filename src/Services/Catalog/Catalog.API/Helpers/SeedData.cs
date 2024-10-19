using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Helpers
{
    public static class SeedData
    {
        public static void MigrateData(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "an error occured while seeding database");
            }
        }
    }
}
