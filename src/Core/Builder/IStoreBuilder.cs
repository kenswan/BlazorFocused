using System;

namespace BlazoRx.Core.Builder
{
    public interface IStoreBuilder<TState>
    {
        IStoreBuilder<TState> RegisterAction(Type type);
        IStoreBuilder<TState> RegisterReducer(Type type);
    }
}
