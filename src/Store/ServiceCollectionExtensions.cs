﻿using System;
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
            var builder = new StoreBuilder<T>();

            if (builderFunction is not null)
            {
                builderFunction(builder);
            }

            services.AddHttpClient<IRestClient, RestClient>();

            services.AddScoped<IStore<T>, Store<T>>(serviceProvider =>
            {
                var httpClient = serviceProvider.GetRequiredService<IRestClient>();
                var store = new Store<T>(initialData, httpClient);

                store.LoadBuilder(builder);

                return store;
            });
        }
    }
}
