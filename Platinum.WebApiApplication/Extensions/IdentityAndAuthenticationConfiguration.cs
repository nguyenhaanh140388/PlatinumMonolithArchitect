using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platinum.Core.Settings;
using System.Text;

namespace Platinum.WebApiApplication.Extensions
{
    public static class IdentityAndAuthenticationConfiguration
    {
        public static void RegisterIdentityAndAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            //{
            //    services.AddDbContext<Anhny010920AdministratorContext>(options =>
            //        options.UseInMemoryDatabase("IdentityDb"));
            //}
            //else
            //{
            //    services.AddDatabaseContext<Anhny010920AdministratorContext>(configuration.GetConnectionString("Anhny010920Administrator"));
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
            //    .AddEntityFrameworkStores<Anhny010920AdministratorContext>()
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
