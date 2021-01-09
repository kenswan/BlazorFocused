using System;
using System.Reactive.Subjects;
using Microsoft.Extensions.DependencyInjection;

namespace BlazoRx.Core.Store
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;
        private IServiceProvider serviceProvider;
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

        public void Dispose()
        {
            state.Dispose();
        }

        public void AddReducer<TType>() where TType : Type
        {
            serviceCollection.AddTransient<TType>();
        }

        public void GetReducer<TType>() where TType : Type
        {
            serviceCollection.AddTransient<TType>();
        }

        public void Build()
        {
            serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
