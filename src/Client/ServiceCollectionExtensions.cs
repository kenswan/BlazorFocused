using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRestClient(this IServiceCollection services, string baseUrl) 
        {
            services.AddTransient<IUrlParameterBuilder, UrlParameterBuilder>();

            services.AddHttpClient<IRestClient, RestClient>(client =>
                client.BaseAddress = new Uri(baseUrl));
        }
            

        public static void AddRestClient(
            this IServiceCollection services,
            Action<HttpClient> configureClient = null)
        {
            services.AddTransient<IUrlParameterBuilder, UrlParameterBuilder>();

            if (configureClient is null)
            {
                services.AddHttpClient<IRestClient, RestClient>();
            }
            else
            {
                services.AddHttpClient<IRestClient, RestClient>(configureClient);
            }

        }
    }
}
