// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using BlazorFocused.Tools.Utility;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BlazorFocused.Store;

public partial class StoreTests
{
    [Fact(DisplayName = "Should execute async action")]
    public async Task ShouldRetrieveValueAsyncByInstance()
    {
        SimpleClass originalClass = SimpleClassUtilities.GetRandomSimpleClass();
        SimpleClass updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
        var testServiceMock = new Mock<TestService>();

        testServiceMock.Setup(service =>
            service.GetValueAsync<SimpleClass>())
                .ReturnsAsync(updatedClass);

        using var serviceProvider = serviceCollection
            .AddTransient<TestActionAsync>()
            .AddTransient(sp => testServiceMock.Object)
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalClass, serviceProvider);

        await store.DispatchAsync<TestActionAsync>();

        store.GetState().Should().BeEquivalentTo(updatedClass);
        testServiceMock.Verify(service => service.GetValueAsync<SimpleClass>(), Times.Once);
    }

    [Fact(DisplayName = "Should execute async action with input by instance")]
    public async Task ShouldRetrieveValueAsyncWithInputByInstance()
    {
        string input = new Faker().Random.String();
        SimpleClass originalClass = SimpleClassUtilities.GetRandomSimpleClass();
        SimpleClass updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
        var testServiceMock = new Mock<TestService>();

        testServiceMock.Setup(service =>
            service.GetValueAsync<string, SimpleClass>(input))
                .ReturnsAsync(updatedClass);

        using var serviceProvider = serviceCollection
            .AddTransient<TestActionAsyncWithInput>()
            .AddTransient(sp => testServiceMock.Object)
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalClass, serviceProvider);

        await store.DispatchAsync<TestActionAsyncWithInput, string>(input);

        store.GetState().Should().BeEquivalentTo(updatedClass);

        testServiceMock.Verify(service =>
            service.GetValueAsync<string, SimpleClass>(input), Times.Once);
    }
}
