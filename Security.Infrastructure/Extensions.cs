using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Security.Application.Services;
using Security.Infrastructure.EntityFramework;
using Security.Infrastructure.Security;
using Security.Infrastructure.Services;
using System.Text;

namespace Security.Infrastructure
{
    public static class Extensions
    {
        public static void AddSecurity(IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<SecurityDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthorization(config =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .Build();

                config.DefaultPolicy = defaultAuthPolicy;

                foreach (var mnemonic in ApplicationPermission.GetAllPermissions().Select(x => x.Mnemonic))
                {
                    config.AddPolicy(mnemonic,
                        policy => policy.RequireClaim("Permission", new string[] { mnemonic }));
                }
            });

            JwtOptions jwtoptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.AddAuthentication().AddJwtBearer("Bearer", jwtOptions =>
            {
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey));
                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = jwtoptions.ValidateIssuer,
                    ValidateAudience = jwtoptions.ValidateAudience,
                    ValidIssuer = jwtoptions.ValidIssuer,
                    ValidAudience = jwtoptions.ValidAudience
                };
            });
            System.Diagnostics.Debug.WriteLine(jwtoptions==null?true:false);
            services.AddSingleton(jwtoptions);


            services.AddScoped<SecurityInitializer>();
            services.AddScoped<ISecurityService, SecurityService>();

        }

        /*public static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnectionString");

            services.AddDbContext<SecurityDbContext>(ctx =>
                ctx.UseSqlServer(connectionString));


            services.AddHostedService<DbInitializer>();
        }*/

        public static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {

            /*var connectionString = configuration.GetConnectionString("DbConnectionString");

            services.AddDbContext<SecurityDbContext>(ctx =>
                ctx.UseMySql(connectionString, ));// Asegúrate de especificar la versión correcta de MySQL

            services.AddHostedService<DbInitializer>();*/
            var connectionString = configuration.GetConnectionString("MySqlConnection");

            services.AddDbContext<SecurityDbContext>(ctx =>
                ctx.UseMySql(connectionString, new Microsoft.EntityFrameworkCore.MySqlServerVersion(new Version(8, 0, 26))));


            services.AddHostedService<DbInitializer>();
        }


        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            AddDatabase(services, configuration);
            AddSecurity(services, configuration);

            return services;
        }
    }
}
