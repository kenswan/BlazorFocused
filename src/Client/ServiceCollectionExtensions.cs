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
            Action<HttpClient> configureClient = null)
        {
            services.ConfigureRestClientOptions();

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
            services.ConfigureRestClientOptions();
            services.AddSingleton(sp => new OAuthToken());

            if (configureClient is null)
            {
                return services.AddHttpClient<IOAuthRestClient, OAuthRestClient>();
            }
            else
            {
                return services.AddHttpClient<IOAuthRestClient, OAuthRestClient>(configureClient);
            }
        }

        private static void ConfigureRestClientOptions(this IServiceCollection services)
        {
            services
                .AddOptions<RestClientOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection(nameof(RestClient)).Bind(options);
                })
                .Validate(options =>
                {
                    if (!string.IsNullOrWhiteSpace(options.BaseAddress))
                    {
                        if (!Uri.TryCreate(options.BaseAddress, UriKind.Absolute, out _))
                        {
                            throw new RestClientException("BaseAddress in configuration is not a valid Uri");
                        }
                    }

                    return true;
                });
        }
    }
}
