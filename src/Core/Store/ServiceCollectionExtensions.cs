using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Core.Store
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStore<T>(this IServiceCollection services) where T : class
        {
            services.AddScoped<IStore<T>>();
        }
    }
}
