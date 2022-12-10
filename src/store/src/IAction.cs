// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused;

/// <summary>
/// Performs an action that can be triggered by the store
/// </summary>
/// <typeparam name="TState">State of the store data</typeparam>
public interface IAction<TState> : IStoreAction<TState>
{
    /// <summary>
    /// This method is executed by the store chain when dispatched
    /// </summary>
    /// <returns>New state of store after action is complete</returns>
    TState Execute();
}

/// <summary>
/// Performs an action that can be triggered by the store
/// </summary>
/// <typeparam name="TState">State of the store data</typeparam>
/// <typeparam name="TInput">Input object at time of execution</typeparam>
public interface IAction<TState, TInput> : IStoreAction<TState>
{
    /// <summary>
    /// This method is executed by the store chain when dispatched
    /// </summary>
    /// <param name="input">Input object passed at time of dispatch</param>
    /// <returns>New state of store after action is complete</returns>
    TState Execute(TInput input);
}
