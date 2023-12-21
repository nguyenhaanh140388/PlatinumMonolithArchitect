using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using static Platinum.Core.Common.Constants;
using ILogger = Serilog.ILogger;

namespace Platinum.WebApiApplication.Extensions
{
    public static class SerialLogConfiguration
    {
        public static void AddSerialLog(this IServiceCollection services, IConfiguration config)
        {
            // Serilog.
            var columnOption = new ColumnOptions();
            columnOption.Store.Remove(StandardColumn.MessageTemplate);
            services.AddSingleton<ILogger>(x =>
            {
                return new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Platinum", LogEventLevel.Information)
                .WriteTo.MSSqlServer(config.GetConnectionString(ConnectionStringNames.Catalog), SerilogProperties.TableName, columnOptions: columnOption)
                .CreateLogger();
            });
        }
    }
}
