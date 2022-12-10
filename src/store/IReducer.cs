// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused;

/// <summary>
/// Reduces/transforms store state for specific application purpose/component
/// </summary>
/// <typeparam name="TInput">Original state of store</typeparam>
/// <typeparam name="TOutput">Reduced/transformed state</typeparam>
public interface IReducer<TInput, TOutput> where TInput : class
{
    /// <summary>
    /// This method returns a reduced/transformed form of the original state
    /// </summary>
    /// <param name="input">Original state of store</param>
    /// <returns>Reduced/transformed state</returns>
    TOutput Execute(TInput input);
}
