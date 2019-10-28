using System;
using System.Reactive.Subjects;

namespace BlazoRx.Core
{
    public class FilteredState<TState, TReduced> : IDisposable
    {
        private readonly Func<TState, TReduced> filter;
        private readonly IObservable<TState> observableState;
        private BehaviorSubject<TReduced> reducedState;
        private IDisposable subscription;

        public FilteredState(IObservable<TState> observableState, Func<TState, TReduced> filter)
        {
            this.filter = filter;
            this.observableState = observableState;

            reducedState = new BehaviorSubject<TReduced>(default);
        }

        public IObservable<TReduced> Reduce()
        {
            subscription = observableState.Subscribe(state => reducedState.OnNext(filter(state)));

            return reducedState;
        }

        public void Dispose()
        {
            // Unsubscribe
            subscription.Dispose();

            reducedState.Dispose();
        }
    }
}
