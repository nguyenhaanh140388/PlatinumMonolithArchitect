using Autofac;

namespace Platinum.Identity
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                  .Where(c => c.FullName!.StartsWith("Platinum.Identity.Infrastructure") ||
                  c.FullName.StartsWith("Platinum.Identity.Core")
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
