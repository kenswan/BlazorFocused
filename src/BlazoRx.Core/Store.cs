using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace BlazoRx.Core
{
    public class Store<T> : IStore<T> where T : class
    {
        private BehaviorSubject<T> state;

        public Store()
        {
            state = new BehaviorSubject<T>(default);
        }
        
        public void Update(T state)
        {
            this.state.OnNext(state);
        }

        public IObservable<T> Connect()
        {
            return state;
        }

        public IObservable<TReducedValue> Connect<TReducedValue>(IReducer<T, TReducedValue> reducer)
        {
            throw new NotImplementedException();
        }
    }
}
