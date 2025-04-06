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
                    var HomeDb = sp.GetRequiredService<ConnectionStringsOptions>();

                    string postgresql = HomeDb.ConnectionString;
                    // var connectionStringsOptions =
                    //     sp.GetRequiredService<ConnectionStringsOptions>();
                    // options.UseSqlServer(configuration.GetConnectionString("HomeDb"));

                    options.UseNpgsql(postgresql);
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
