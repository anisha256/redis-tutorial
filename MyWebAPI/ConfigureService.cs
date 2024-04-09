using MyWebAPI.Services;
using StackExchange.Redis;

namespace MyWebAPI
{
    public static  class ConfigureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfiguration = ConfigurationOptions.Parse(configuration["Redis:ConnectionString"]);
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });

            services.AddScoped<IRedisCacheService,RedisCacheService>();

            return services;
        }

    }
}
