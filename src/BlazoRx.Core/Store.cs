using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace BlazoRx.Core
{
    public class Store<T> : IStore<T>, IDisposable where T : class
    {
        private readonly BehaviorSubject<T> state;

        public Store(T initialState = default)
        {
            state = new BehaviorSubject<T>(initialState);
        }

        public IObservable<T> Connect()
        {
            return state;
        }

        public FilteredState<T, TReduced> Connect<TReduced>(Func<T, TReduced> filter)
        {
            return new FilteredState<T, TReduced>(state, filter);
        }

        public void Dispatch(Func<T, T> action)
        {
            Update(action(state.Value));
        }

        public T GetCurrentState()
        {
            return state.Value;
        }

        private void Update(T state)
        {
            this.state.OnNext(state);
        }
        
        public void Dispose()
        {
            state.Dispose();
        }
    }
}
