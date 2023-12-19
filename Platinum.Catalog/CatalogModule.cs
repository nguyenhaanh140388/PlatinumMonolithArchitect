using Autofac;

namespace Platinum.Catalog
{
    public class CatalogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                  .Where(c => c.FullName!.StartsWith("Platinum.Catalog.Infrastructure") ||
                  c.FullName.StartsWith("Platinum.Catalog.Core")
                  )
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            //               .Where(c => c.FullName.StartsWith("Platinum.Catalog.Jobs")
            //               )
            //               .AsSelf()
            //               .InstancePerLifetimeScope();

        }
    }


}
