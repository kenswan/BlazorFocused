﻿namespace BlazorFocused.Store
{
    /// <inheritdoc cref="IAction{TState}"/>
    public abstract class StoreAction<TState> : IAction<TState>
    {
        public TState State { get; set; }

        public abstract TState Execute();
    }

    /// <inheritdoc cref="IAction{TState, TInput}"/>
    public abstract class StoreAction<TState, TInput> : IAction<TState, TInput>
    {
        public TState State { get; set; }

        public abstract TState Execute(TInput input);
    }
}
