using System.Text.Json;

namespace Platinum.WebApiApplication.Extensions
{
    public static class JsonConfiguration
    {
        public static void AddJsonConfiguration(this IServiceCollection service)
        {
            service.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
        }
    }
}
