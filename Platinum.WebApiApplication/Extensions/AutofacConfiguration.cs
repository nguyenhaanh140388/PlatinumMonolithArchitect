using Autofac;
using Autofac.Extensions.DependencyInjection;
using Platinum.Core.Utils;
using System.Reflection;

namespace Platinum.WebApiApplication.Extensions
{
    public static class AutofacConfiguration
    {
        public static void AddAutofac(this ConfigureHostBuilder builder, IServiceCollection services, IConfiguration config)
        {
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            var builderContainer = new Autofac.ContainerBuilder();
            builder.ConfigureContainer<ContainerBuilder>(builderContainer =>
           {
               //Assembly.Load("Platinum.Infrastructure");
               //Assembly.Load("Platinum.Core");
               //Assembly.Load("Platinum.Catalog");

               builderContainer.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                              .Where(
                                      c => c.FullName!.StartsWith("Platinum.Infrastructure") ||
                                      c.FullName.StartsWith("Platinum.Core.Commands")
                                     )
                              .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

               //  builderContainer.Populate(services);

               //  var build = builderContainer.Build();
               //  var registrations = build.ComponentRegistry.Registrations;
               //  var dup = registrations.GroupBy(x => x)
               //.Where(g => g.Count() > 1)
               //.Select(y => y.Key)
               //.ToList();

               

               services.RegisterDependenciesInModules(builderContainer, config);
           });


        }
    }
}
