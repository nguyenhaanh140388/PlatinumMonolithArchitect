using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platinum.Catalog.Extensions;
using Platinum.Core.Modular;

namespace Platinum.Catalog
{
    public class ModuleInitializer : ModuleBase
    {

        public ModuleInitializer()
            : base()
        {

        }

        protected override void RegisterServices(IServiceCollection serviceCollection, ContainerBuilder builderContainer, IConfiguration config)
        {
            base.RegisterServices(serviceCollection, builderContainer, config);
            builderContainer.RegisterModule(new CatalogModule());
            serviceCollection
               .AddCore(config)
               .AddShopInfrastructure(config, builderContainer);
        }

        protected override async Task RegisterJobsAsync(IServiceProvider serviceProvider)
        {
            //var hello = serviceProvider.GetService<HelloWorld>();
            //await hello.Excute();
            await base.RegisterJobsAsync(serviceProvider);
        }
    }
}
