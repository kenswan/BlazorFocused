using System;
using BlazoRx.Core.Reducer;

namespace BlazoRx.Core.Builder
{
    public interface IStoreBuilder<TState> where TState : class
    {
        IServiceProvider Build();
        void RegisterAction(Action<TState> action);
        void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer);
    }
}
