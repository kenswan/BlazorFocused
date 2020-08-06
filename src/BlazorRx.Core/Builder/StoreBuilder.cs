using System;
namespace BlazorRx.Core.Builder
{
    public class StoreBuilder<TState> : IStoreBuilder<TState> where TState : class
    {
        public IStoreBuilder<TState> RegisterAction(Type type)
        {
            throw new NotImplementedException();
        }

        public IStoreBuilder<TState> RegisterReducer(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
