using System;
using System.Threading.Tasks;

namespace BlazorFocused.Store
{
    /// <summary>
    /// Representation of a specific store operating within the context of the application.
    /// The store can dispatch actions, store, update, and reduce state.
    /// </summary>
    /// <typeparam name="TState">Original state within the store</typeparam>
    public interface IStore<TState> where TState : class
    {
        /// <summary>
        /// Sends registered action to the store to operate on current state
        /// </summary>
        /// <typeparam name="TAction">Action registered in store
        /// See <see cref="IAction{TState}"/>
        /// </typeparam>
        void Dispatch<TAction>()
            where TAction : IAction<TState>;

        /// <summary>
        /// Sends registered action to the store to operate on current state
        /// </summary>
        /// <typeparam name="TAction">Action registered in store
        /// <typeparam name="TInput">Input object sent at time of execution</typeparam>
        /// See <see cref="IAction{TState,TInput}"/>
        /// </typeparam>
        void Dispatch<TAction, TInput>(TInput input)
            where TAction : IAction<TState, TInput>;

        /// <summary>
        /// Sends registered asynchronous action to the store to operate on current state
        /// </summary>
        /// <typeparam name="TActionAsync">Asynchronous action registered in store
        /// See <see cref="IActionAsync{TState}"/></typeparam>
        /// <returns>Awaitable task</returns>
        ValueTask DispatchAsync<TActionAsync>()
            where TActionAsync : IActionAsync<TState>;

        /// <summary>
        /// Sends registered asynchronous action to the store to operate on current state
        /// </summary>
        /// <typeparam name="TActionAsync">Asynchronous action registered in store</typeparam>
        /// <typeparam name="TInput">Input object sent at time of execution</typeparam>
        /// See <see cref="IActionAsync{TState,TInput}"/></typeparam>
        /// <returns>Awaitable task</returns>
        ValueTask DispatchAsync<TActionAsync, TInput>(TInput input)
            where TActionAsync : IActionAsync<TState, TInput>;

        /// <summary>
        /// Returns current value of the state within the current store
        /// </summary>
        /// <returns>Current state of the store</returns>
        TState GetState();

        /// <summary>
        /// Retrieves reduced/transformed version of original store state
        /// </summary>
        /// <typeparam name="TOutput">Reduced/transformed state</typeparam>
        /// <param name="action">Performed when initial or future reduced value
        /// of state is received</param>
        /// <remarks>
        /// When inside a Blazor component, this method will usually end with StateHasChanged()
        /// to update consumer components dependent upon that state
        /// (ComponentBase.StateHasChanged)
        /// </remarks>
        void Reduce<TReducer, TOutput>(Action<TOutput> action)
            where TOutput : class
            where TReducer : class, IReducer<TState, TOutput>;

        /// <summary>
        /// Updates current state of store
        /// </summary>
        /// <param name="updatedState">New value of store state</param>
        void SetState(TState updatedState);

        /// <summary>
        /// Updates current state of store
        /// </summary>
        /// <param name="updateFunction">Inline function used to update state value</param>
        void SetState(Func<TState, TState> updateFunction);

        /// <summary>
        /// Subscribes to current and future values of store state
        /// </summary>
        /// <param name="action">Peformed when initial state or updates are received</param>
        /// <remarks>
        /// When inside a Blazor component, this method will usually end with StateHasChanged()
        /// to update consumer components dependent upon that state
        /// (ComponentBase.StateHasChanged)
        /// </remarks>
        void Subscribe(Action<TState> action);
    }
}
