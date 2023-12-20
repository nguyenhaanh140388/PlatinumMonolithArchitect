﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Platinum.Infrastructure.Data.EmailTemplates.Seeds;
using Serilog;

namespace Platinum.Infrastructure.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task<IWebHost> SeedTemplateData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await EmailTemplateInitialize.Initialize(services);
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
