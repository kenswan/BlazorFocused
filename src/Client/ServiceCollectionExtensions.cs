using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRestClient<T>(this IServiceCollection services, Uri uri) where T : class
        {
            services.AddHttpClient<IRestClient, RestClient>(client =>
                client.BaseAddress = uri);
        }
    }
}
