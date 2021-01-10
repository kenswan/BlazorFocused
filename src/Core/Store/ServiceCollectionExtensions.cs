using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Core.Store
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStore<T>(this IServiceCollection services, T initialData) where T : class
        {
            var store = new Store<T>(initialData);

            services.AddScoped<IStore<T>, Store<T>>(ServiceProvider => store);
        }
    }
}
