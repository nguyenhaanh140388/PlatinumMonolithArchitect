using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Platinum.Catalog.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }

        public static IServiceCollection AddShopInfrastructure(this IServiceCollection services, IConfiguration config, ContainerBuilder builderContainer)
        {
            //var servicesInternal = new ServiceCollection();
            //servicesInternal
            //    .AddDbContext<Anhny010920CatalogContext>(options => options.UseSqlServer(config.GetConnectionString("Anhny010920Catalog")))
            //    .AddScoped<IAnhny010920CatalogContext>(provider => provider.GetService<Anhny010920CatalogContext>());

            //builderContainer.Populate(servicesInternal);

            return services;
        }
    }
}