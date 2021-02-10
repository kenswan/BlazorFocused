using System;
using BlazorFocused.Client;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStore<T>(
            this IServiceCollection services,
            T initialData,
            Action<IStoreBuilder<T>> builderFunction = null) where T : class
        {
            IStoreBuilder<T> builder = new StoreBuilder<T>();

            if (builderFunction is not null)
            {
                builderFunction(builder);
            }

            services.AddScoped<IStore<T>, Store<T>>(serviceProvider =>
            {
                var store = new Store<T>(initialData, builder);

                return store;
            });
        }
    }
}
