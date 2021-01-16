using System;

namespace BlazorFocused.Store
{
    public interface IStoreBuilder<TState> where TState : class
    {
        IServiceProvider Build();

        void RegisterAction(IAction<TState> action);

        void RegisterAsyncAction(IActionAsync<TState> action);

        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer);
    }
}
