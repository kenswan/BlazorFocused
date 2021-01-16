using System;
using System.Threading.Tasks;

namespace BlazorFocused.Store
{
    public interface IStore<TState> where TState : class
    {
        void Dispatch<TAction>()
            where TAction : IAction<TState>;

        ValueTask DispatchAsync<TAction>()
            where TAction : IActionAsync<TState>;

        TState GetState();

        void Reduce<TOutput>(Action<TOutput> action);

        void SetState(TState updatedState);

        void SetState(Func<TState, TState> updateFunction);

        void Subscribe(Action<TState> action);
    }
}
