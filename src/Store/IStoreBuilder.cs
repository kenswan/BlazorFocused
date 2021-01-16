using System;

namespace BlazorFocused.Store
{
    public interface IStoreBuilder<TState> where TState : class
    {
        IServiceProvider BuildServices();

        void RegisterAction(IAction<TState> action);

        void RegisterAsyncAction(IActionAsync<TState> action);

        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer);
    }
}
