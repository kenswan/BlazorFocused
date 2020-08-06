using System;
using System.Reactive.Subjects;

namespace BlazorRx.Core.Store
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;

        public Store(T initialState = default)
        {
            state = new BehaviorSubject<T>(initialState);
        }

        public T GetCurrentState()
        {
            return state.Value;
        }

        public void Dispose()
        {
            state.Dispose();
        }
    }
}
