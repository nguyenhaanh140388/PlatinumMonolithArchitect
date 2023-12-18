// <copyright file="Anhny010920WebsiteExtension.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.MiddleWares;
using Microsoft.AspNetCore.Builder;

namespace Anhny010920.Core.Extensions
{
    /// <summary>
    /// Anhny010920WebsiteExtension.
    /// </summary>
    public static class Anhny010920WebsiteExtension
    {
        /// <summary>
        /// Uses the anhny010920 website header validator.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>Result.</returns>
        public static IApplicationBuilder UseAnhny010920WebsiteHeaderValidator(this IApplicationBuilder app)
        {
            app.UseMiddleware<Anhny010920WebsiteHeaderValidator>();
            return app;
        }

        public static IApplicationBuilder UseMaintenanceWebsite(this IApplicationBuilder app)
        {
            app.UseMiddleware<MaintenanceWebsite>();
            return app;
        }
    }
}
