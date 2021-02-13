using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStore<T>(
            this IServiceCollection services, T initialData) where T : class
        {
            services.AddScoped<IStore<T>, Store<T>>(serviceProvider =>
                new Store<T>(initialData));
        }

        public static void AddStore<T>(
            this IServiceCollection services,
            T initialData,
            Action<IStoreBuilder<T>> builderFunction) where T : class
        {
            IStoreBuilder<T> builder = new StoreBuilder<T>();

            builderFunction(builder);

            services.AddScoped<IStore<T>, Store<T>>(serviceProvider =>
                new Store<T>(initialData, builder));
        }
    }
}
