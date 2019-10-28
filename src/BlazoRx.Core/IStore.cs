using System;
using System.Collections.Generic;
using System.Text;

namespace BlazoRx.Core
{
    public interface IStore<T> where T : class
    {
        IObservable<T> Connect();

        FilteredState<T, TReduced> Connect<TReduced>(Func<T, TReduced> filter);

        void Dispatch(Func<T, T> action);
    }
}
