using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Platinum.Identity.Infrastructure.Seeds;
using Serilog;

namespace Platinum.Identity.Infrastructure.Extensions
{
    public static class SeedDataExtesion
    {
        public static async Task<IWebHost> SeedIdentity(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //var context = services.GetRequiredService<PlatinumCatalogContext>();
                    await IdentitytInitialize.Initialize(services);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger>();
                    logger.Error("An error occurred while seeding the database");
                }
            }

            return host;
        }

    }
}
