using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Tools.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(
            this IServiceCollection services,
            Dictionary<string, string> appSettings = default)
        {
            var configurationBuilder = new ConfigurationBuilder();

            if (appSettings is not null)
            {
                configurationBuilder.AddInMemoryCollection(appSettings);
            }

            return services.AddSingleton<IConfiguration>(configurationBuilder.Build());
        }
    }
}
