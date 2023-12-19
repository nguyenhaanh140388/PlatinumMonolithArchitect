using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platinum.Core.Abstractions.Modules;

namespace Platinum.Core.Modular
{
    /// <summary>
    /// ModuleBase.
    /// </summary>
    /// <seealso cref="IModuleInitializer" />
    public abstract class ModuleBase : IModuleInitializer
    {
        protected IServiceProvider _serviceProvider;
        protected IConfiguration _config;
        protected ContainerBuilder _container;
        //private static readonly Assembly assembly = Assembly.GetCallingAssembly();

        public ModuleBase()
        {
            //this.serviceProvider = serviceProvider;
            //this.config = config;
        }

        /// <summary>
        /// Initializes the specified service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Init(IServiceCollection serviceCollection,
            ContainerBuilder container,
            IConfiguration config)
        {
            RegisterServices(serviceCollection,
             container, config
             );
        }

        /// <summary>
        /// Initializes the specified service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AutoTasks(IServiceProvider serviceProvider)
        {
            await RegisterJobsAsync(serviceProvider);
        }

        /// <summary>
        /// Register Register menu links.
        /// </summary>
        protected virtual void RegisterServices(IServiceCollection serviceCollection,
            ContainerBuilder container,
            IConfiguration config
            )
        {

        }

        protected virtual async Task RegisterJobsAsync(IServiceProvider serviceProvider)
        {
            await Task.FromResult<string>(null);
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget Job Executed"));

            //// delayed job example
            //BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executed"), TimeSpan.FromMinutes(1));

            //// recurring job example
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Minutely Job executed"), Cron.Minutely);

            // Continuations job example
            //var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            //BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));
        }
    }
}
