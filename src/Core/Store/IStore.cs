using System;
using BlazoRx.Core.Action;

namespace BlazoRx.Core.Store
{
    public interface IStore<TState> where TState : class
    {
        void Dispatch<TAction>()
            where TAction : IAction<TState>;

        TState GetState();

        void Reduce<TOutput>(Action<TOutput> action);

        void SetState(TState updatedState);

        void SetState(Func<TState, TState> updateFunction);

        void Subscribe(Action<TState> action);
    }
}
