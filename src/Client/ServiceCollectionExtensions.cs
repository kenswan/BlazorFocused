using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRestClient(this IServiceCollection services, string baseUrl)
        {
            services.AddHttpClient<IRestClient, RestClient>(client =>
                client.BaseAddress = new Uri(baseUrl));
        }

        public static void AddRestClient(this IServiceCollection services, Action<HttpClient> configureClient)
        {
            if (configureClient != null)
            {
                services.AddHttpClient<IRestClient, RestClient>(configureClient);
            }
        }
    }
}
