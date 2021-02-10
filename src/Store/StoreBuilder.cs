using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    public class StoreBuilder<TState> : IStoreBuilder<TState> where TState : class
    {
        private Action<HttpClient> configureHttpClient;
        private ServiceCollection serviceCollection;

        public StoreBuilder()
        {
            serviceCollection = new ServiceCollection();
            configureHttpClient = null;
        }

        public void RegisterAction(IAction<TState> action)
        {
            serviceCollection.AddTransient(action.GetType(), provider => action);
        }

        public void RegisterAction<TAction>()
            where TAction : IAction<TState>
        {
            Type type = typeof(TAction);

            serviceCollection.AddTransient(type);
        }

        public void RegisterAsyncAction(IActionAsync<TState> action)
        {
            serviceCollection.AddTransient(action.GetType(), provider => action);
        }

        public void RegisterAsyncAction<TAction>()
            where TAction : IActionAsync<TState>
        {
            Type type = typeof(TAction);

            serviceCollection.AddTransient(type);
        }

        public void RegisterHttpClient()
        {
            serviceCollection.AddHttpClient();
        }

        public void RegisterHttpClient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            serviceCollection.AddHttpClient<TService, TImplementation>();
        }

        public void RegisterHttpClient<TService, TImplementation>(Action<HttpClient> configureHttpClient)
            where TService : class
            where TImplementation : class, TService
        {
            serviceCollection.AddHttpClient<TService, TImplementation>(configureHttpClient);
        }

        public void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer)
        {
            serviceCollection.AddTransient(provider => reducer);
        }

        public IServiceProvider BuildServices()
        {
            return serviceCollection.BuildServiceProvider();
        }

        public Action<HttpClient> BuildHttpClient()
            => configureHttpClient;

        public void RegisterService<TService>() where TService : class
        {
            Type type = typeof(TService);

            serviceCollection.AddScoped(type);
        }

        public void RegisterService<TService>(TService service) where TService : class
        {
            serviceCollection.AddScoped(provider => service);
        }
    }
}
