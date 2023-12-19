using Autofac;
using Platinum.Core.Abstractions.Modules;
using Platinum.Core.Modular;
using System.Reflection;

namespace Platinum.WebApiApplication.Extensions
{
    public static class DependenciesInModulesConfiguration
    {
        public static void RegisterDependenciesInModules(this IServiceCollection services,
            ContainerBuilder builderContainer,
            IConfiguration config)
        {
            Assembly.Load("Platinum.Infrastructure");
            Assembly.Load("Platinum.Core");
            Assembly.Load("Platinum.Catalog");

            var types = AppDomain.CurrentDomain
              .GetAssemblies()
              .SelectMany(x => x.GetTypes()
              .Where(t => t.IsSubclassOf(typeof(ModuleBase))));

            foreach (var type in types)
            {
                // Register dependency in modules
                var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(type)!;
                moduleInitializer.Init(services, builderContainer, config);
            }
        }
    }
}
