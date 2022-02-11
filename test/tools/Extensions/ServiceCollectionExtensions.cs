using BlazorFocused.Tools.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

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

        public static IServiceProvider BuildProviderWithTestLogger<T>(
            this IServiceCollection serviceCollection, ITestOutputHelper testOutputHelper)
        {
            var previousLogger = serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(ILogger<>));

            serviceCollection.Remove(previousLogger);

            serviceCollection.AddSingleton<ILogger<T>>(new TestLogger<T>((level, message, exception) =>
                testOutputHelper.WriteTestLoggerMessage(level, message, exception)));

            return serviceCollection.BuildServiceProvider();
        }
    }
}
