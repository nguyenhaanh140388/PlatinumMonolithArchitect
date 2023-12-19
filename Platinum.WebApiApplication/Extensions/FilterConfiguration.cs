﻿using Anhny010920WebAppApi.Filters.ActionFilters;
using Anhny010920WebAppApi.Filters.ResourceFilters;
using Anhny010920WebAppApi.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Platinum.Core.Exceptions;

namespace Platinum.WebApiApplication.Extensions
{
    public static class FilterConfiguration
    {
        public static void AddFilterExtension(this IServiceCollection services, IConfiguration config)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // User as global filter.
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<LoggingActionFilter>();
                options.Filters.Add<WebApiExceptionFilter>();
                //options.Filters.Add(new CorsAuthorizationFilterFactory(CorsProperties.DefaultPolicy));
            });

            services.AddScoped(container =>
            {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

                return new ClientIpCheckActionFilter(
                    config["AdminSafeList"], logger);
            });

            services.AddScoped<LoggingActionFilter>();
            services.AddScoped<WebApiExceptionFilter>();
            services.AddScoped<CustomAsyncResourceCacheFilter>();
            services.AddScoped<CustomAsyncResultFilter>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }
    }
}
