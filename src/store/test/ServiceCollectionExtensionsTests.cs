﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused;

public class ServiceCollectionExtensionsTests
{
    [Fact(DisplayName = "Should register store with initial state")]
    public void ShouldRegisterStore()
    {
        var simpleClass = new SimpleClass { FieldOne = "Test" };
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddStore(simpleClass);
        using ServiceProvider provider = serviceCollection.BuildServiceProvider();

        IStore<SimpleClass> store = provider.GetRequiredService<IStore<SimpleClass>>();

        Assert.NotNull(store);
        Assert.Equal(simpleClass.FieldOne, store.GetState().FieldOne);
    }
}
