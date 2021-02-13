using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;
        private readonly IServiceProvider internalServiceProvider;

        public Store(T initialState, IStoreBuilder<T> storeBuilder = null)
        {
            state = new BehaviorSubject<T>(initialState);
            internalServiceProvider = storeBuilder?.BuildServices();
        }

        public Store(T initialState, Action<IStoreBuilder<T>> storeBuilderAction)
        {
            state = new BehaviorSubject<T>(initialState);
            var storeBuilder = new StoreBuilder<T>();

            storeBuilderAction.Invoke(storeBuilder);
            internalServiceProvider = storeBuilder.BuildServices();
        }

        public void Dispatch<TAction>() where TAction : IAction<T>
        {
            var action = internalServiceProvider.GetRequiredService<TAction>();

            state.OnNext(action.Execute(state.Value));
        }

        public async ValueTask DispatchAsync<TActionAsync>() where TActionAsync : IActionAsync<T>
        {
            var action = internalServiceProvider.GetRequiredService<TActionAsync>();

            var value = await action.ExecuteAsync(state.Value);

            state.OnNext(value);
        }

        public T GetState()
        {
            return state.Value;
        }

        public void Reduce<TOutput>(Action<TOutput> action)
        {
            var reducer = internalServiceProvider.GetRequiredService<IReducer<T, TOutput>>();

            state.Subscribe(data =>
            {
                action(reducer.Execute(data));
            });
        }

        public void SetState(T value)
        {
            state.OnNext(value);
        }

        public void SetState(Func<T, T> updateFunction)
        {
            state.OnNext(updateFunction(state.Value));
        }

        public void Subscribe(Action<T> action)
        {
            state.Subscribe(data => action(data));
        }

        public void Dispose()
        {
            state.Dispose();
        }
    }
}
