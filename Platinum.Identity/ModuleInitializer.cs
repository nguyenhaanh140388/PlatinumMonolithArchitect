using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platinum.Core.Modular;

namespace Platinum.Identity
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
            builderContainer.RegisterModule(new IdentityModule());
            //serviceCollection
            //   .AddCore(config)
            //   .AddShopInfrastructure(config, builderContainer);
        }

        protected override async Task RegisterJobsAsync(IServiceProvider serviceProvider)
        {
            //var hello = serviceProvider.GetService<HelloWorld>();
            //await hello.Excute();
            await base.RegisterJobsAsync(serviceProvider);
        }
    }
}
