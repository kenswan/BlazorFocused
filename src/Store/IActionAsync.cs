using System.Threading.Tasks;
using BlazorFocused.Client;

namespace BlazorFocused.Store
{
    /// <summary>
    /// Performs an asynchronous action that can be triggered by the store
    /// </summary>
    /// <typeparam name="TState">State of the store data</typeparam>
    public interface IActionAsync<TState>
    {
        /// <summary>
        /// This method is asynchronously executed by the store chain when dispatched
        /// </summary>
        /// <param name="state">Current state of the store</param>
        /// <returns>New state of store after action is complete</returns>
        ValueTask<TState> ExecuteAsync(TState state);
    }
}
