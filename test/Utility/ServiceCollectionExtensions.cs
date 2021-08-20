using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Utility
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEmptyConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        }
    }
}
