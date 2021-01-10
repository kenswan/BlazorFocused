using System;

namespace BlazoRx.Core.Store
{
    public interface IStore<TState> where TState : class
    {
        TState GetState();

        void Reduce<TOutput>(Action<TOutput> action);

        void SetState(TState updatedState);

        void SetState(Func<TState, TState> updateFunction);

        void Subscribe(Action<TState> action);
    }
}
