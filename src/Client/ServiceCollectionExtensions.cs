using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace BlazorFocused.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddRestClient(this IServiceCollection services, string baseUrl) =>
            services.AddRestClient(client => client.BaseAddress = new Uri(baseUrl));

        public static IHttpClientBuilder AddRestClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient = null) =>
                (configureClient is null) ?
                services.AddHttpClient<IRestClient, RestClient>((serviceProvider, httpClient) =>
                    Configure(serviceProvider, httpClient)) :
                services.AddHttpClient<IRestClient, RestClient>(configureClient);

        public static IHttpClientBuilder AddOAuthRestClient(this IServiceCollection services, string baseUrl) =>
            services.AddOAuthRestClient(client => client.BaseAddress = new Uri(baseUrl));

        public static IHttpClientBuilder AddOAuthRestClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient = null)
        {
            services.AddSingleton(sp => new OAuthToken());

            if (configureClient is null)
            {
                return services.AddHttpClient<IOAuthRestClient, OAuthRestClient>((serviceProvider, httpClient) =>
                    Configure(serviceProvider, httpClient));
            }
            else
            {
                return services.AddHttpClient<IOAuthRestClient, OAuthRestClient>(configureClient);
            }
        }

        private static void Configure(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();

            if (configuration is not null && configuration.GetSection(nameof(RestClient)).Exists())
            {
                RestClientSettings restClientSettings = new();

                configuration.GetSection(nameof(RestClient)).Bind(restClientSettings);

                if (restClientSettings is not null)
                {
                    httpClient.ConfigureRestClientSettings(restClientSettings);
                }

            }
        }
    }
}
