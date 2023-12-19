using Autofac;

namespace Platinum.Infrastructure.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterSharedModules(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(c =>
                c.FullName.StartsWith("Anhny010920.Infrastructure") ||
                c.FullName.StartsWith("Anhny010920.Core.Commands"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
