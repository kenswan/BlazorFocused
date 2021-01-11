using System;

namespace BlazoRx.Store
{
    public interface IStoreBuilder<TState> where TState : class
    {
        IServiceProvider Build();
        void RegisterAction(IAction<TState> action);
        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer);
    }
}
