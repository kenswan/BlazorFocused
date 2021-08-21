using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            Action<HttpClient> configureClient = null)
        {
            services.AddRestClientOptions<RestClient>(registerDefault: true);

            return (configureClient is null) ?
                services.AddHttpClient<IRestClient, RestClient>() :
                services.AddHttpClient<IRestClient, RestClient>(configureClient);
        }

        public static IHttpClientBuilder AddOAuthRestClient(this IServiceCollection services, string baseUrl) =>
            services.AddOAuthRestClient(client => client.BaseAddress = new Uri(baseUrl));

        public static IHttpClientBuilder AddOAuthRestClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient = null)
        {
            services.AddRestClientOptions<OAuthRestClient>();
            services.TryAddSingleton(sp => new OAuthToken());
            services.TryAddTransient<RestClientAuthHandler>();

            return (configureClient is null) ?
                services.AddHttpClient<IOAuthRestClient, OAuthRestClient>()
                    .AddHttpMessageHandler<RestClientAuthHandler>() :
                services.AddHttpClient<IOAuthRestClient, OAuthRestClient>(configureClient)
                    .AddHttpMessageHandler<RestClientAuthHandler>();
        }

        private static void AddRestClientOptions<T>(
            this IServiceCollection services, bool registerDefault = false)
            where T : IRestClient
        {
            var configKey = typeof(T).Name;
            var defaultKey = nameof(RestClient);
            var namedOption = registerDefault == true ? "" : typeof(T).Name;

            services
                .AddOptions<RestClientOptions>(namedOption)
                .Configure<IConfiguration>((options, configuration) =>
                    (configuration.GetSection(configKey) switch
                    {
                        IConfigurationSection section when section.Exists() => section,
                        _ => configuration.GetSection(defaultKey)
                    }).Bind(options))
                .Validate(options =>
                    string.IsNullOrWhiteSpace(options.BaseAddress) ||
                        Uri.TryCreate(options.BaseAddress, UriKind.Absolute, out _),
                    "BaseAddress in configuration is not a valid Uri");
        }
    }
}
