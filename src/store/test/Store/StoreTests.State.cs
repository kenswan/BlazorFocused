﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store;

public partial class StoreTests
{
    [Fact(DisplayName = "Should update state")]
    public void ShouldUpdateState()
    {
        var originalState = new SimpleClass { FieldOne = "Original" };
        var expectedState = new SimpleClass { FieldOne = "Expected" };

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalState, serviceProvider);

        store.SetState(expectedState);

        SimpleClass actualState = store.GetState();

        actualState.Should().BeEquivalentTo(expectedState);
    }

    [Fact(DisplayName = "Should update state with method")]
    public void ShouldUpdateStateWithMethod()
    {
        var originalState = new SimpleClass { FieldOne = "Original" };
        var expectedState = new SimpleClass { FieldOne = "Expected" };

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalState, serviceProvider);

        store.SetState(currentState =>
        {
            currentState.FieldOne = "Expected";
            return currentState;
        });

        SimpleClass actualState = store.GetState();

        actualState.Should().BeEquivalentTo(expectedState);
    }

    [Fact(DisplayName = "Should subscribe to state")]
    public void ShouldSubscribeState()
    {
        var originalState = new SimpleClass { FieldOne = "Original" };
        var expectedState = new SimpleClass { FieldOne = "Expected" };
        SimpleClass updatedState = null;

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalState, serviceProvider);

        store.Subscribe((newState) => { updatedState = newState; });

        store.GetState().Should().BeEquivalentTo(originalState);

        store.SetState(expectedState);

        updatedState.Should().BeEquivalentTo(expectedState);
    }
}
