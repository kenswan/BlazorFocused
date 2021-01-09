using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Core.Builder
{
    public class StoreBuilder<TState> : IStoreBuilder<TState> where TState : Type
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
