// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reactive.Subjects;

namespace BlazorFocused.Store;

/// <inheritdoc cref="IStore{TState}"/>
internal class Store<TState> : IStore<TState>, IDisposable where TState : class
{
    private readonly BehaviorSubject<TState> state;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<Store<TState>> logger;
    private readonly string storeName;

    public Store(TState initialState, IServiceProvider serviceProvider)
    {
        storeName = typeof(TState).ToString();

        this.serviceProvider = serviceProvider;

        state = new BehaviorSubject<TState>(initialState);

        logger = serviceProvider.GetService<ILogger<Store<TState>>>() ??
            NullLogger<Store<TState>>.Instance;
    }

    public void Dispatch<TAction>() where TAction : IAction<TState>
    {
        Type actionName = typeof(TAction);

        logger.LogDebug("Retrieving action {ActionName}", actionName);

        using IServiceScope scope = serviceProvider.CreateScope();

        TAction action = scope.ServiceProvider.GetRequiredService<TAction>();

        logger.LogDebug("Found action {ActionName}", actionName);

        action.State = state.Value;

        logger.LogInformation("Executing action {ActionName}", actionName);

        state.OnNext(action.Execute());
    }

    public void Dispatch<TAction, TInput>(TInput input)
        where TAction : IAction<TState, TInput>
    {
        Type actionName = typeof(TAction);

        logger.LogDebug("Retrieving action {ActionName}", actionName);

        using IServiceScope scope = serviceProvider.CreateScope();

        TAction action = scope.ServiceProvider.GetRequiredService<TAction>();

        logger.LogDebug("Found action {ActionName}", actionName);

        action.State = state.Value;

        logger.LogInformation("Executing action {ActionName}", actionName);

        state.OnNext(action.Execute(input));
    }

    public async ValueTask DispatchAsync<TActionAsync>() where TActionAsync : IActionAsync<TState>
    {
        Type actionName = typeof(TActionAsync);

        logger.LogDebug("Retrieving action {ActionName}", actionName);

        using IServiceScope scope = serviceProvider.CreateScope();

        TActionAsync action = scope.ServiceProvider.GetRequiredService<TActionAsync>();

        logger.LogDebug("Found action {ActionName}", actionName);

        action.State = state.Value;

        logger.LogInformation("Executing action {ActionName}", actionName);

        TState value = await action.ExecuteAsync();

        state.OnNext(value);
    }
    public async ValueTask DispatchAsync<TActionAsync, TInput>(TInput input)
        where TActionAsync : IActionAsync<TState, TInput>
    {
        Type actionName = typeof(TActionAsync);

        logger.LogDebug("Retrieving action {ActionName}", actionName);

        using IServiceScope scope = serviceProvider.CreateScope();

        TActionAsync action = scope.ServiceProvider.GetRequiredService<TActionAsync>();

        logger.LogDebug("Found action {ActionName}", actionName);

        action.State = state.Value;

        logger.LogInformation("Executing action {ActionName}", actionName);

        TState value = await action.ExecuteAsync(input);

        state.OnNext(value);
    }

    public TState GetState() => state.Value;

    public void Reduce<TReducer, TOutput>(Action<TOutput> action)
        where TOutput : class
        where TReducer : class, IReducer<TState, TOutput>
    {
        Type reducerName = typeof(TOutput);

        logger.LogDebug("Retrieving reducer {ReducerName}", reducerName);

        using IServiceScope scope = serviceProvider.CreateScope();

        TReducer reducer = scope.ServiceProvider.GetRequiredService<TReducer>();

        logger.LogDebug("Found reducer {ReducerName}", reducerName);

        logger.LogDebug("Setting subscription for {ReducerName}", reducerName);

        state.Subscribe(data =>
        {
            logger.LogInformation("Executing reducer {ReducerName}", reducerName);

            action(reducer.Execute(data));
        });
    }

    public void SetState(TState updatedState)
    {
        logger.LogDebug("Setting state for store {StoreName}", storeName);

        state.OnNext(updatedState);
    }

    public void SetState(Func<TState, TState> updateFunction)
    {
        logger.LogDebug("Setting state for store {StoreName}", storeName);

        state.OnNext(updateFunction(state.Value));
    }

    public void Subscribe(Action<TState> action)
    {
        logger.LogDebug("Subscribing to store {StoreName}", storeName);

        state.Subscribe(data => action(data));
    }

    public void Dispose() => state.Dispose();
}
