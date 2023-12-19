using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Platinum.WebApiApplication.Extensions
{
    public static class CultureInfoConfiguration
    {
        public static void UseCultureInfo(this IApplicationBuilder app)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy",
                LongDatePattern = "MM/dd/yyyy hh:mm:ss tt"
            };

            culture.DateTimeFormat = dateformat;

            var supportedCultures = new[]
            {
                culture
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}
