// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace BlazorFocused.Store;

public partial class StoreTests
{
    protected readonly ServiceCollection serviceCollection;
    private readonly ITestOutputHelper testOutputHelper;

    public StoreTests(ITestOutputHelper testOutputHelper)
    {
        serviceCollection = new();
        this.testOutputHelper = testOutputHelper;
    }

    [Fact(DisplayName = "Should Store and Return Initial Value")]
    public void ShouldStoreAndReturnInitialValue()
    {
        SimpleClass inputSimpleClass = SimpleClassUtilities.GetRandomSimpleClass();
        SimpleClass expectedSimpleClass = inputSimpleClass;

        using var serviceProvider = new ServiceCollection()
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(inputSimpleClass, serviceProvider);

        SimpleClass actualSimpleClass = store.GetState();

        actualSimpleClass.Should().BeEquivalentTo(expectedSimpleClass);
    }

    [Fact(DisplayName = "Should Return 'null' when initialized as null")]
    public void ShouldReturnNullWhenInitializedAsNull()
    {
        using var serviceProvider = new ServiceCollection()
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(null, serviceProvider);

        SimpleClass actualSimpleClass = store.GetState();

        actualSimpleClass.Should().BeNull();
    }
}
