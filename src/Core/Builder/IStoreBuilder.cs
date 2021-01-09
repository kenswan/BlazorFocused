using System;
using BlazorRx.Core.Reducer;

namespace BlazorRx.Core.Builder
{
    public interface IStoreBuilder<TState>
    {
        IStoreBuilder<TState> RegisterAction(Type type);
        IStoreBuilder<TState> RegisterReducer(Type type);
    }
}
