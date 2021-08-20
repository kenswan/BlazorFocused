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
            services.AddRestClientOptions(nameof(RestClient));

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
            services.AddRestClientOptions(nameof(OAuthRestClient), nameof(OAuthRestClient));
            services.TryAddSingleton(sp => new OAuthToken());
            services.TryAddTransient<RestClientAuthHandler>();

            if (configureClient is null)
            {
                return services.AddHttpClient<IOAuthRestClient, OAuthRestClient>()
                    .AddHttpMessageHandler<RestClientAuthHandler>();
            }
            else
            {
                return services.AddHttpClient<IOAuthRestClient, OAuthRestClient>(configureClient)
                    .AddHttpMessageHandler<RestClientAuthHandler>();
            }
        }

        private static void AddRestClientOptions(
            this IServiceCollection services,
            string configurationSection,
            string namedOption = "")
        {
            services
                .AddOptions<RestClientOptions>(namedOption)
                .Configure<IConfiguration>((options, configuration) =>
                {
                    if(configuration.GetSection(configurationSection).Exists())
                    {
                        configuration.GetSection(configurationSection).Bind(options);
                    }
                    else
                    {
                        // Default to basic RestClient properties if not found
                        configuration.GetSection(nameof(RestClient)).Bind(options);
                    }
                })
                .Validate(options =>
                {
                    if (!string.IsNullOrWhiteSpace(options.BaseAddress))
                    {
                        if (!Uri.TryCreate(options.BaseAddress, UriKind.Absolute, out _))
                        {
                            return false;
                        }
                    }

                    return true;
                }, "BaseAddress in configuration is not a valid Uri");
        }
    }
}
