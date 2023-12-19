using static Platinum.Core.Common.Constants;

namespace Platinum.WebApiApplication.Extensions
{
    public static class DistributedSqlServerCacheConfiguration
    {
        public static void AddDistributedSqlServerCacheExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddMemoryCache();
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = config.GetConnectionString(ConnectionStringNames.Catalog);
                options.SchemaName = DistributedSqlServerCache.SchemaName;
                options.TableName = DistributedSqlServerCache.TableName;
            });
        }
    }
}
