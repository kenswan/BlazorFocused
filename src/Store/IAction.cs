namespace BlazorFocused.Store
{
    /// <summary>
    /// Performs an action that can be triggered by the store
    /// </summary>
    /// <typeparam name="TState">State of the store data</typeparam>
    public interface IAction<TState>
    {
        /// <summary>
        /// This method is executed by the store chain when dispatched
        /// </summary>
        /// <param name="state">Current state of the store</param>
        /// <returns>New state of store after action is complete</returns>
        TState Execute(TState state);
    }
}
