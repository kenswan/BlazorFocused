using System;
using BlazoRx.Core.Action;
using BlazoRx.Core.Reducer;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Core.Builder
{
    public class StoreBuilder<TState> : IStoreBuilder<TState> where TState : class
    {
        private ServiceCollection serviceCollection;

        public StoreBuilder()
        {
            serviceCollection = new ServiceCollection();
        }

        public void RegisterAction(IAction<TState> action)
        {
            serviceCollection.AddTransient(action.GetType());
        }

        public void RegisterReducer<TOutput>(IReducer<TState, TOutput> reducer)
        {
            serviceCollection.AddTransient(provider => reducer);
        }

        public IServiceProvider Build()
        {
            return serviceCollection.BuildServiceProvider();
        }
    }
}
