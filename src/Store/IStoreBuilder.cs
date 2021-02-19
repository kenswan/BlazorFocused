using System;
using System.Net.Http;

namespace BlazorFocused.Store
{
    /// <summary>
    /// Builds/assembles actions, reducers, services and state for store to operate on
    /// </summary>
    /// <typeparam name="TState">State of the store</typeparam>
    public interface IStoreBuilder<TState> where TState : class
    {
        /// <summary>
        /// Exports compiled services that have been registered within store
        /// </summary>
        /// <returns>Collection of store service objects</returns>
        IServiceProvider BuildServices();

        /// <summary>
        /// Registers action (by type) that can operate within store.
        /// See <see cref="IAction{TState}"/>
        /// </summary>
        /// <typeparam name="TAction">Type of action being registered</typeparam>
        void RegisterAction<TAction>()
            where TAction : IAction<TState>;

        /// <summary>
        /// Registers action (by instance) that can operate within store.
        /// See <see cref="IAction{TState}"/>
        /// </summary>
        /// <param name="action">Initialized instance of action registered</param>
        void RegisterAction(IAction<TState> action);

        /// <summary>
        /// Registers async action (by type) that can operate within store.
        /// See <see cref="IActionAsync{TState}"/>
        /// </summary>
        /// <typeparam name="TAction">Type of asynchronous action being registered</typeparam>
        void RegisterAsyncAction<TAction>()
            where TAction : IActionAsync<TState>;

        /// <summary>
        /// Registers async action (by instance) that can operate within store.
        /// See <see cref="IActionAsync{TState}"/>
        /// </summary>
        /// <param name="action">Initialized instance of action registered</param>
        void RegisterAsyncAction(IActionAsync<TState> action);

        /// <summary>
        /// Registers HttpClient that can be injected into services and async actions
        /// within store
        /// </summary>
        void RegisterHttpClient();

        /// <summary>
        /// Registers typed HttpClient that can be injected into services and async actions
        /// within store
        /// </summary>
        /// <typeparam name="TInterface">Typed client interface</typeparam>
        /// <typeparam name="TImplementation">Concrete implementation of typed client</typeparam>
        /// <remarks>See <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients">
        /// Microsoft Docs - Typed Clients
        /// </see>
        /// </remarks>
        void RegisterHttpClient<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface;

        /// <summary>
        /// Registers typed HttpClient that can be injected into services and async actions
        /// within store 
        /// </summary>
        /// <typeparam name="TInterface">Typed client interface</typeparam>
        /// <typeparam name="TImplementation">Concrete implementation of typed client</typeparam>
        /// <param name="configureHttpClient">Performs HttpClient property updates/configuration</param>
        /// <remarks>See <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients">
        /// Microsoft Docs - Typed Clients
        /// </see>
        /// </remarks>
        void RegisterHttpClient<TInterface, TImplementation>(Action<HttpClient> configureHttpClient)
            where TInterface : class
            where TImplementation : class, TInterface;

        /// <summary>
        /// Registers reducer (by type) that can operate within the store
        /// </summary>
        /// <typeparam name="TReducer">Type of requested Reducer.
        /// See <see cref="IReducer{TInput, TOutput}"/>.</typeparam>
        /// <typeparam name="TOutput">Reduced/transformed store state</typeparam>
        void RegisterReducer<TReducer, TOutput>()
            where TOutput : class
            where TReducer : class, IReducer<TState, TOutput>;

        /// <summary>
        /// Registers reducer (by instance) that can operate within the store
        /// </summary>
        /// <typeparam name="TOutput">Reduced/transformed store state.
        /// See <see cref="IReducer{TInput, TOutput}"/>.</typeparam>
        /// <param name="reducer">Initialized instance of reducer registered</param>
        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer)
            where TOutput : class;

        /// <summary>
        /// Register service (by type) that can operate within store actions
        /// </summary>
        /// <typeparam name="TService">Type of store being registered</typeparam>
        void RegisterService<TService>()
            where TService : class;

        /// <summary>
        /// Register service (by instance) that can operate within store actions
        /// </summary>
        /// <typeparam name="TService">Type of store being registered</typeparam>
        /// <param name="service">Initialized instance of service registered</param>
        void RegisterService<TService>(TService service)
            where TService : class;

        /// <summary>
        /// Set initial or default value of store state
        /// </summary>
        /// <param name="state">Store state</param>
        void SetInitialState(TState state);
    }
}
