using Platinum.Core.Abstractions.Services;
using Platinum.Core.Settings;
using Platinum.Infrastructure.Services;

namespace Platinum.WebApiApplication.Extensions
{
    public static class SharedServicesConfiguration
    {
        public static void RegisterSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient(typeof(IDataShaper<>), typeof(DataShaperService<>));
        }
    }
}
