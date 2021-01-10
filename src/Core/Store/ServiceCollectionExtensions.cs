using System;
using BlazoRx.Core.Builder;
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

        public static void AddStore<T>(
            this IServiceCollection services,
            T initialData,
            Func<IStoreBuilder<T>, IStoreBuilder<T>> builderFunction) where T : class
        {
            var store = new Store<T>(initialData);

            var builder = builderFunction(new StoreBuilder<T>());

            store.LoadBuilder(builder);

            services.AddScoped<IStore<T>, Store<T>>(ServiceProvider => store);
        }
    }
}
