using System;
using System.Reactive.Subjects;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorFocused.Store
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;
        private IServiceProvider internalServiceProvider;

        public Store(T initialState = default)
        {
            state = new BehaviorSubject<T>(initialState);
        }

        public void Dispatch<TAction>() where TAction : IAction<T>
        {
            var action = internalServiceProvider.GetRequiredService<TAction>();

            state.OnNext(action.Execute(state.Value));
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
            internalServiceProvider = builder.Build();
        }

        public void Dispose()
        {
            state.Dispose();
        }
    }
}
