using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Platinum.Core.Abstractions.Modules;
using Platinum.Core.Abstractions.Services;
using Platinum.Core.Modular;
using Platinum.Core.Settings;
using Platinum.Infrastructure.Services;
using System.Text;

namespace Platinum.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services,
            IConfiguration config,
            IMvcBuilder mvcBuilder)
        {
            //LoadModules(env);

            //var types = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(x => x.GetTypes().Where(t => t.IsSubclassOf(typeof(ModuleBase))));
            //foreach (var type in types)
            //{
            //    // Register dependency in modules
            //    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(type, services, config, containerBuilder);
            //}

            mvcBuilder
                .AddRazor()
                .AddJson(services);

            //services.AddAutoMapper();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.RegisterIdentityAndAuthentication(config);
            services.RegisterSharedServices(config);

            return services;
        }

        //public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        //{
        //    var mappingConfig = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AllowNullCollections = true;
        //        var profiles = AppDomain.CurrentDomain.GetAssemblies()
        //        .SelectMany(x => x.GetTypes())
        //        .Where(x => x.IsSubclassOf(typeof(MapperProfileBase)));
        //        cfg.AddMaps(profiles);
        //    });

        //    // mappingConfig.CompileMappings();
        //    IMapper mapper = mappingConfig.CreateMapper();
        //    services.AddSingleton(mapper);
        //    return services;
        //}

        public static IMvcBuilder AddJson(this IMvcBuilder mvcBuilder, IServiceCollection services)
        {
            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //});

            return mvcBuilder;
        }

        public static IMvcBuilder AddRazor(this IMvcBuilder mvcBuilder)
        {
            //mvcBuilder.AddRazorOptions(o =>
            //{
            //    foreach (var module in GlobalConfiguration.Modules)
            //    {
            //        o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(module.Assembly.Location));
            //    }
            //});

            return mvcBuilder;
        }

        ///// <summary>
        ///// Loads the installed modules.
        ///// </summary>
        //public static void LoadModules(IHostingEnvironment env)
        //{
        //    var moduleRootFolder = new DirectoryInfo(env.ContentRootPath);

        //    if (!moduleRootFolder.Exists)
        //    {
        //        return;
        //    }

        //    foreach (var file in moduleRootFolder.GetFileSystemInfos("Platinum.Shop.dll", SearchOption.AllDirectories))
        //    {
        //        Assembly assembly = null;
        //        try
        //        {
        //            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
        //        }
        //        catch (FileLoadException ex)
        //        {
        //            if (ex.Message == "Assembly with same name is already loaded")
        //            {
        //                // Get loaded assembly
        //                assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));
        //            }
        //            else
        //            {
        //                throw;
        //            }

        //            if (assembly == null)
        //            {
        //                throw;
        //            }
        //        }

        //        GlobalConfiguration.Modules.Add(new ModuleInfo
        //        {
        //            Name = file.Name,
        //            WebRootPath = Path.Combine(moduleRootFolder.FullName, file.Name.Split('.')[2].ToLower(), "wwwroot"),
        //            Assembly = assembly,
        //            Path = moduleRootFolder.FullName,
        //        });
        //    }
        //}

        //public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
        //{
        //    services.AddControllers()
        //    .ConfigureApplicationPartManager(manager =>
        //    {
        //        manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
        //    });
        //    return services;
        //}

        public static void RegisterIdentityAndAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            //{
            //    services.AddDbContext<PlatinumAdministratorContext>(options =>
            //        options.UseInMemoryDatabase("IdentityDb"));
            //}
            //else
            //{
            //    services.AddDatabaseContext<PlatinumAdministratorContext>(configuration.GetConnectionString("PlatinumAdministrator"));
            //}

            //services.AddIdentity<ApplicationUser, ApplicationRole>(
            //    opts =>
            //    {
            //        opts.Password.RequiredLength = 8;
            //        opts.Password.RequireDigit = false;
            //        opts.Password.RequireLowercase = false;
            //        opts.Password.RequireUppercase = false;
            //        opts.Password.RequireNonAlphanumeric = false;
            //        opts.SignIn.RequireConfirmedEmail = true;
            //    })
            //    .AddEntityFrameworkStores<PlatinumAdministratorContext>()
            //    .AddDefaultTokenProviders()
            //    .AddUserManager<AppUserManager>();

            services.Configure<PasswordHasherOptions>(options =>
            options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = configuration["JWTSettings:ValidAudience"],
                        ValidIssuer = configuration["JWTSettings:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Secret"]))
                    };
                });
        }

        public static void RegisterSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            // best practice  
            services.AddTransient(typeof(IDataShaper<>), typeof(DataShaperService<>));
        }

        public static void RegisterDependenciesInModules(this IServiceCollection services,
            ContainerBuilder container,
            IConfiguration config)
        {
            var types = AppDomain.CurrentDomain
              .GetAssemblies()
              .SelectMany(x => x.GetTypes()
              .Where(t => t.IsSubclassOf(typeof(ModuleBase))));

            foreach (var type in types)
            {
                // Register dependency in modules
                var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(type);
                moduleInitializer.Init(services, container, config);
            }
        }

        public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services, string connectionString) where T : DbContext
        {
            services.AddMSSQL<T>(connectionString);
            return services;
        }

        private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString) where T : DbContext
        {
            services.AddDbContext<T>(m => m.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
            //using var scope = services.BuildServiceProvider().CreateScope();
            //var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            //dbContext.Database.Migrate();
            return services;
        }
    }
}
