using Microsoft.Extensions.DependencyInjection;

namespace Platinum.Infrastructure.Identity.Seeds
{
    public class IdentitytInitialize
    {
        public async static Task Initialize(IServiceProvider services)
        {
            try
            {
                // Get a logger
                var logger = services.GetRequiredService<ILogger>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                await DefaultBasicUser.SeedAsync(userManager);

                Log.Information("Finished Seeding Default Data");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "An error occurred seeding the DB");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
