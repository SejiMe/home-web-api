using Microsoft.AspNetCore.Identity;
using web.api.Infrastructure.Data;
using web.api.Interfaces;
using web.api.Services;

namespace web.api.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommon(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<ConnectionStringsOptions>(configuration.GetSection("HomeDb"));
            services.AddSingleton<ConnectionStringsOptions>(sp =>
                sp.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value
            );

            return services;
        }

        public static IServiceCollection AddEfCore(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<HomeDbContext>(
                (sp, options) =>
                {
                    // string connectionString =
                    //     "Host=localhost;Port=5432;Username=seji;Password=dev;Database=HomeDb";

                    string sqlServerConnectionstring =
                        @"Server=.;Database=HomeDb;Trusted_Connection=True;TrustServerCertificate=True;";
                    var connectionStringsOptions =
                        sp.GetRequiredService<ConnectionStringsOptions>();
                    options.UseSqlServer(sqlServerConnectionstring);
                    // options.UseNpgsql(connectionString);
                }
            );
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddAuthorization();

            services
                .AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

            services
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<HomeDbContext>()
                .AddApiEndpoints();
            return services;
        }
    }
}
