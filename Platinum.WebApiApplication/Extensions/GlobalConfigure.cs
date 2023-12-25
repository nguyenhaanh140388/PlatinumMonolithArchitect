using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography.Xml;
using static Platinum.Core.Common.Constants;

namespace Platinum.WebApiApplication.Extensions
{
    public static class GlobalConfigure
    {
        public static void AddGlobalExtension(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // options.FileProviders.Clear();
                //options.FileProviders.Add(new PhysicalFileProvider(appDirectory));
                var modules = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains(SolutionNameSpace));
                foreach (var module in modules)
                {
                    // options.FileProviders.Add(new EmbeddedFileProvider(module));
                }
            });

            // configure strongly typed settings object
            //services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));
            services.Configure<KeyInfo>(config.GetSection(KeyInfoProperties.ConfigName));

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None;
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddDirectoryBrowser();

            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.MultipartBodyLengthLimit = 268435456;
            });
            // services.Configure<KestrelServerOptions>(this.Configuration.GetSection("Kestrel"));
        }

    }
}
