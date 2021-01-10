using System;
using System.Reactive.Subjects;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Core.Store
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;
        private IServiceProvider internalServiceProvider;
        private readonly IServiceCollection serviceCollection;

        public Store(T initialState = default)
        {
            state = new BehaviorSubject<T>(initialState);
            serviceCollection = new ServiceCollection();
        }

        public T GetCurrentState()
        {
            return state.Value;
        }

        public void SetState(Func<T, T> updateFunction)
        {
            state.OnNext(updateFunction(state.Value));
        }

        public void Dispose()
        {
            state.Dispose();
        }

        public void Build()
        {
            internalServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
