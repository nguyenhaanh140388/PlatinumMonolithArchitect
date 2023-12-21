using Platinum.Core.Models;
using static Platinum.Core.Common.Constants;

namespace Platinum.WebApiApplication.Extensions
{
    public static class CorsConfiguration
    {
        public static void AddCorsExtension(this IServiceCollection services, IConfiguration config)
        {
            List<Cors> cors = config.GetSection(CorsProperties.ConfigName).Get<List<Cors>>();

            services.AddCors(options =>
            {
                //if (app.Environment.IsDevelopment())
                //{
                options.AddPolicy("PlatinumWebAppApi",
                    builder =>
                    {
                        builder.WithOrigins(
                            "http://localhost:4200",
                            "http://localhost:59739",
                            "http://localhost:3000"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                //}
                //else
                //{
                //    foreach (var cor in cors)
                //    {
                //        var origins = cor.Origins.Split(",");
                //        var methods = cor.Methods.Split(",");

                //        options.AddPolicy(
                //             cor.PolicyName,
                //             builder =>
                //             {
                //                 builder
                //                 .WithOrigins(origins)
                //                 .WithMethods(methods)
                //                 .AllowAnyHeader()
                //                 .AllowCredentials();
                //             });

                //    }
                //}

                // options.DefaultPolicyName = "DefaultAppCors";
            });
        }
    }
}
