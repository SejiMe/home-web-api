using web.api.Infrastructure.Data;

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
                    string connectionString =
                        "Host=localhost;Port=5432;Username=seji;Password=dev;Database=HomeDb";
                    var connectionStringsOptions =
                        sp.GetRequiredService<ConnectionStringsOptions>();
                    options.UseNpgsql(connectionString);
                }
            );
            return services;
        }
    }
}
