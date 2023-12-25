using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platinum.Catalog.Core.Abstractions.EntityFramework;
using Platinum.Catalog.Core.Entities;

namespace Platinum.Catalog.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, ContainerBuilder builderContainer)
        {
            var servicesInternal = new ServiceCollection();
            servicesInternal
                .AddDbContext<PlatinumCatalogContext>(options => options.UseSqlServer(config.GetConnectionString("PlatinumCatalog")))
                .AddScoped<IPlatinumCatalogContext>(provider => provider.GetService<PlatinumCatalogContext>());

            builderContainer.Populate(servicesInternal);

            return services;
        }
    }
}