using System;
using System.Net.Http;

namespace BlazorFocused.Store
{
    public interface IStoreBuilder<TState> where TState : class
    {
        Action<HttpClient> BuildHttpClient();

        IServiceProvider BuildServices();

        void RegisterAction<TAction>()
            where TAction : IAction<TState>;

        void RegisterAction(IAction<TState> action);

        void RegisterAsyncAction<TAction>()
            where TAction : IActionAsync<TState>;

        void RegisterAsyncAction(IActionAsync<TState> action);

        void RegisterHttpClient();

        void RegisterHttpClient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void RegisterHttpClient<TService, TImplementation>(Action<HttpClient> configureHttpClient)
            where TService : class
            where TImplementation : class, TService;

        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer);

        void RegisterService<TService>()
            where TService : class;

        void RegisterService<TService>(TService service)
            where TService : class;
    }
}
