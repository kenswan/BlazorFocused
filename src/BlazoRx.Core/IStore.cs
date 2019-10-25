using System;
using System.Collections.Generic;
using System.Text;

namespace BlazoRx.Core
{
    public interface IStore<T> where T : class
    {
        IObservable<T> Connect();

        IObservable<TReducedValue> Connect<TReducedValue>(IReducer<T, TReducedValue> reducer);

        void Update(T State);
    }
}
