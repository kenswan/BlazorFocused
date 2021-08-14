using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace BlazorFocused.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRestClient(this IServiceCollection services, string baseUrl) =>
            services.AddRestClient(client => client.BaseAddress = new Uri(baseUrl));

        public static void AddRestClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient = null)
        {
            if (configureClient is null)
            {
                services.AddHttpClient<IRestClient, RestClient>();
            }
            else
            {
                services.AddHttpClient<IRestClient, RestClient>(configureClient);
            }
        }

        public static void AddOauthRestClient(this IServiceCollection services, string baseUrl) =>
            services.AddOAuthRestClient(client => client.BaseAddress = new Uri(baseUrl));

        public static void AddOAuthRestClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient = null)
        {
            services.AddSingleton(sp => new OAuthToken());

            if (configureClient is null)
            {
                services.AddHttpClient<IOAuthRestClient, OAuthRestClient>();
            }
            else
            {
                services.AddHttpClient<IOAuthRestClient, OAuthRestClient>(configureClient);
            }
        }
    }
}
