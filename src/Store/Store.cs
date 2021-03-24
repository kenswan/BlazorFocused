using System;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Store
{
    /// <inheritdoc cref="IStore{TState}"/>
    internal class Store<TState> : IStore<TState>, IDisposable where TState : class
    {
        private readonly BehaviorSubject<TState> state;
        private readonly IServiceProvider internalServiceProvider;
        private readonly ILogger<Store<TState>> logger;
        private readonly string storeName;

        public Store(Action<IStoreBuilder<TState>> storeBuilderAction)
        {
            storeName = typeof(TState).ToString();

            var storeBuilder = new StoreBuilder<TState>();

            if (storeBuilderAction is not null)
            {
                storeBuilderAction.Invoke(storeBuilder);
            }

            state = new BehaviorSubject<TState>(storeBuilder.InitialState);

            internalServiceProvider = storeBuilder.BuildServices();

            logger = internalServiceProvider.GetService<ILogger<Store<TState>>>() ??
                NullLogger<Store<TState>>.Instance;
        }

        public void Dispatch<TAction>() where TAction : IAction<TState>
        {
            var actionName = typeof(TAction);

            logger.LogDebug($"Retrieving action {actionName}");

            var action = internalServiceProvider.GetRequiredService<TAction>();

            logger.LogDebug($"Found action {actionName}");

            action.State = state.Value;

            logger.LogInformation($"Executing action {actionName}");

            state.OnNext(action.Execute());
        }

        public void Dispatch<TAction, TInput>(TInput input) 
            where TAction : IAction<TState, TInput>
        {
            var actionName = typeof(TAction);

            logger.LogDebug($"Retrieving action {actionName}");

            var action = internalServiceProvider.GetRequiredService<TAction>();

            logger.LogDebug($"Found action {actionName}");

            action.State = state.Value;

            logger.LogInformation($"Executing action {actionName}");

            state.OnNext(action.Execute(input));
        }

        public async ValueTask DispatchAsync<TActionAsync>() where TActionAsync : IActionAsync<TState>
        {
            var actionName = typeof(TActionAsync);

            logger.LogDebug($"Retrieving action {actionName}");

            var action = internalServiceProvider.GetRequiredService<TActionAsync>();

            logger.LogDebug($"Found action {actionName}");

            action.State = state.Value;

            logger.LogInformation($"Executing action {actionName}");

            var value = await action.ExecuteAsync();

            state.OnNext(value);
        }
        public async ValueTask DispatchAsync<TActionAsync, TInput>(TInput input) 
            where TActionAsync : IActionAsync<TState, TInput>
        {
            var actionName = typeof(TActionAsync);

            logger.LogDebug($"Retrieving action {actionName}");

            var action = internalServiceProvider.GetRequiredService<TActionAsync>();

            logger.LogDebug($"Found action {actionName}");

            action.State = state.Value;

            logger.LogInformation($"Executing action {actionName}");

            var value = await action.ExecuteAsync(input);

            state.OnNext(value);
        }

        public TState GetState()
        {
            return state.Value;
        }

        public void Reduce<TOutput>(Action<TOutput> action)
        {
            var reducerName = typeof(TOutput);

            logger.LogDebug($"Retrieving reducer {reducerName}");

            var reducer = internalServiceProvider.GetRequiredService<IReducer<TState, TOutput>>();

            logger.LogDebug($"Found reducer {reducerName}");

            logger.LogDebug($"Setting subscription for {reducerName}");

            state.Subscribe(data =>
            {
                logger.LogInformation($"Executing reducer {reducerName}");

                action(reducer.Execute(data));
            });
        }

        public void SetState(TState updatedState)
        {
            logger.LogDebug($"Setting state for store {storeName}");

            state.OnNext(updatedState);
        }

        public void SetState(Func<TState, TState> updateFunction)
        {
            logger.LogDebug($"Setting state for store {storeName}");

            state.OnNext(updateFunction(state.Value));
        }

        public void Subscribe(Action<TState> action)
        {
            logger.LogDebug($"Subscribing to store {storeName}");

            state.Subscribe(data => action(data));
        }

        public void Dispose()
        {
            state.Dispose();
        }
    }
}
