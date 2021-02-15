using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    internal class StoreBuilder<TState> : IStoreBuilder<TState> where TState : class
    {
        public TState InitialState { get; private set; }

        private readonly ServiceCollection serviceCollection;

        public StoreBuilder()
        {
            InitialState = default;
            serviceCollection = new ServiceCollection();
        }

        public void RegisterAction(IAction<TState> action)
        {
            serviceCollection.AddTransient(action.GetType(), _ => action);
        }

        public void RegisterAction<TAction>()
            where TAction : IAction<TState>
        {
            Type type = typeof(TAction);

            serviceCollection.AddTransient(type);
        }

        public void RegisterAsyncAction(IActionAsync<TState> action)
        {
            serviceCollection.AddTransient(action.GetType(), _ => action);
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
            where TOutput : class
        {
            serviceCollection.AddTransient(_ => reducer);
        }

        public void RegisterReducer<TReducer, TOutput>()
            where TOutput : class
            where TReducer : class, IReducer<TState, TOutput>
        {
            serviceCollection.AddTransient<IReducer<TState, TOutput>, TReducer>();
        }

        public IServiceProvider BuildServices()
        {
            return serviceCollection.BuildServiceProvider();
        }

        public void RegisterService<TService>() where TService : class
        {
            Type type = typeof(TService);

            serviceCollection.AddScoped(type);
        }

        public void RegisterService<TService>(TService service) where TService : class
        {
            serviceCollection.AddScoped(_ => service);
        }

        public void SetInitialState(TState state)
        {
            InitialState = state;
        }
    }
}
