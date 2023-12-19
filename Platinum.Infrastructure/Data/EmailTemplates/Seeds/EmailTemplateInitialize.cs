﻿using Microsoft.Extensions.DependencyInjection;

namespace Platinum.Infrastructure.Data.EmailTemplates.Seeds
{
    public class EmailTemplateInitialize
    {
        public async static Task Initialize(IServiceProvider services)
        {
            try
            {
                // Get a logger
                var logger = services.GetRequiredService<ILogger>();
                var emailTemplateRepository = services.GetRequiredService<IEmailTemplateRepository>();

                await DefaultEmailTemplate.SeedAsync(emailTemplateRepository);

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
