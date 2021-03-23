using System.Threading.Tasks;

namespace BlazorFocused.Store
{
    /// <summary>
    /// Performs an asynchronous action that can be triggered by the store
    /// </summary>
    /// <typeparam name="TState">State of the store data</typeparam>
    public interface IActionAsync<TState> : IStoreAction<TState>
    {
        /// <summary>
        /// This method is asynchronously executed by the store chain when dispatched
        /// </summary>
        /// <returns>New state of store after action is complete</returns>
        ValueTask<TState> ExecuteAsync();
    }

    /// <summary>
    /// Performs an asynchronous action that can be triggered by the store
    /// </summary>
    /// <typeparam name="TState">State of the store data</typeparam>
    /// <typeparam name="TInput">Input object at time of execution</typeparam>
    public interface IActionAsync<TState, TInput> : IStoreAction<TState>
    {
        /// <summary>
        /// This method is asynchronously executed by the store chain when dispatched
        /// </summary>
        /// <param name="input">Input object passed at time of dispatch</param>
        /// <returns>New state of store after action is complete</returns>
        ValueTask<TState> ExecuteAsync(TInput input);
    }
}
