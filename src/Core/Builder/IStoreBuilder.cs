using System;
using BlazoRx.Core.Action;
using BlazoRx.Core.Reducer;

namespace BlazoRx.Core.Builder
{
    public interface IStoreBuilder<TState> where TState : class
    {
        IServiceProvider Build();
        void RegisterAction(IAction<TState> action);
        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer);
    }
}
