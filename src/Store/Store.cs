using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using BlazorFocused.Client;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;
        private readonly IRestClient restClient;
        private IServiceProvider internalServiceProvider;

        public Store(T initialState, IRestClient restClient)
        {
            state = new BehaviorSubject<T>(initialState);
            this.restClient = restClient;
        }

        public void Dispatch<TAction>() where TAction : IAction<T>
        {
            var action = internalServiceProvider.GetRequiredService<TAction>();

            state.OnNext(action.Execute(state.Value));
        }

        public async ValueTask DispatchAsync<TActionAsync>() where TActionAsync : IActionAsync<T>
        {
            var action = internalServiceProvider.GetRequiredService<TActionAsync>();

            var value = await action.ExecuteAsync(restClient, state.Value);

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

        public void LoadBuilder(IStoreBuilder<T> builder)
        {
            internalServiceProvider = builder.BuildServices();
        }

        public void Dispose()
        {
            state.Dispose();
        }
    }
}
