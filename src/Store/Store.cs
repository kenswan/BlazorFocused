using System;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("BlazorFocused.Core.Test")]

namespace BlazorFocused.Store
{
    internal class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;
        private readonly IServiceProvider internalServiceProvider;
        private readonly ILogger<Store<T>> logger;
        private readonly string storeName;

        public Store(Action<IStoreBuilder<T>> storeBuilderAction)
        {
            storeName = typeof(T).ToString();

            var storeBuilder = new StoreBuilder<T>();

            if (storeBuilderAction is not null)
            {
                storeBuilderAction.Invoke(storeBuilder);
            }

            state = new BehaviorSubject<T>(storeBuilder.InitialState);

            internalServiceProvider = storeBuilder.BuildServices();

            this.logger = internalServiceProvider.GetService<ILogger<Store<T>>>();
        }

        public void Dispatch<TAction>() where TAction : IAction<T>
        {
            var actionName = typeof(TAction);

            logger?.LogDebug($"Retrieving action {actionName}");

            var action = internalServiceProvider.GetRequiredService<TAction>();

            logger?.LogDebug($"Found action {actionName}");

            logger?.LogInformation($"Executing action {actionName}");

            state.OnNext(action.Execute(state.Value));
        }

        public async ValueTask DispatchAsync<TActionAsync>() where TActionAsync : IActionAsync<T>
        {
            var actionName = typeof(TActionAsync);

            logger?.LogDebug($"Retrieving action {actionName}");

            var action = internalServiceProvider.GetRequiredService<TActionAsync>();

            logger?.LogDebug($"Found action {actionName}");

            logger?.LogInformation($"Executing action {actionName}");

            var value = await action.ExecuteAsync(state.Value);

            state.OnNext(value);
        }

        public T GetState()
        {
            return state.Value;
        }

        public void Reduce<TOutput>(Action<TOutput> action)
        {
            var reducerName = typeof(TOutput);

            logger?.LogDebug($"Retrieving reducer {reducerName}");

            var reducer = internalServiceProvider.GetRequiredService<IReducer<T, TOutput>>();

            logger?.LogDebug($"Found reducer {reducerName}");

            logger?.LogDebug($"Setting subscription for {reducerName}");

            state.Subscribe(data =>
            {
                logger?.LogInformation($"Executing reducer {reducerName}");

                action(reducer.Execute(data));
            });
        }

        public void SetState(T value)
        {
            logger?.LogDebug($"Setting state for store {storeName}");

            state.OnNext(value);
        }

        public void SetState(Func<T, T> updateFunction)
        {
            logger?.LogDebug($"Setting state for store {storeName}");

            state.OnNext(updateFunction(state.Value));
        }

        public void Subscribe(Action<T> action)
        {
            logger?.LogDebug($"Subscribing to store {storeName}");

            state.Subscribe(data => action(data));
        }

        public void Dispose()
        {
            state.Dispose();
        }
    }
}
