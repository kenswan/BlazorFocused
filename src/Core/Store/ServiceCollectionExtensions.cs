using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Store
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStore<T>(this IServiceCollection services, T initialData) where T : class
        {
            var store = new Store<T>(initialData);

            services.AddScoped<IStore<T>, Store<T>>(ServiceProvider => store);
        }

        public static void AddStore<T>(
            this IServiceCollection services,
            T initialData,
            Action<IStoreBuilder<T>> builderFunction) where T : class
        {
            var store = new Store<T>(initialData);
            var builder = new StoreBuilder<T>();

            builderFunction(builder);

            store.LoadBuilder(builder);

            services.AddScoped<IStore<T>, Store<T>>(ServiceProvider => store);
        }
    }
}
