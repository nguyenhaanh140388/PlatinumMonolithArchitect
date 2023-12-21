using Autofac;

namespace Platinum.Infrastructure.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterSharedModules(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(c =>
                c.FullName!.StartsWith("Platinum.Infrastructure") ||
                c.FullName.StartsWith("Platinum.Core.Commands"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
