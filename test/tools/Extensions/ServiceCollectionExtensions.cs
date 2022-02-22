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
            this IServiceCollection serviceCollection,
            ITestOutputHelper testOutputHelper)
        {
            var previousLogger = serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(ILogger<>));

            serviceCollection.Remove(previousLogger);

            AddTestLoggerToCollection<T>(serviceCollection, testOutputHelper);

            return serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider BuildProviderWithTestLoggers(
            this IServiceCollection serviceCollection,
            Action<IServiceCollection> testLoggers)
        {
            var previousLogger = serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(ILogger<>));

            serviceCollection.Remove(previousLogger);

            testLoggers(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        public static void AddTestLoggerToCollection<T>(
            this IServiceCollection serviceCollection,
            ITestOutputHelper testOutputHelper)
        {
            serviceCollection.AddSingleton<ILogger<T>>(new TestLogger<T>((level, message, exception) =>
                testOutputHelper.WriteTestLoggerMessage(level, message, exception)));
        }
    }
}
