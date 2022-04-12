using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using BlazorFocused.Tools.Utility;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store;

public partial class StoreTests
{
    [Fact(DisplayName = "Should reduce state value with instance")]
    public void ShouldReduceStateValueWithInstance()
    {
        var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
        var originalReducedClass = new TestReducer().Execute(originalClass);
        var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
        var updatedReducedClass = new TestReducer().Execute(updatedClass);

        using var serviceProvider = serviceCollection
            .AddTransient<TestReducer>()
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalClass, serviceProvider);

        SimpleClassSubset actualReducedState = default;

        store.Reduce<TestReducer, SimpleClassSubset>(reducedState =>
        {
            actualReducedState = reducedState;
        });

        actualReducedState.Should().BeEquivalentTo(originalReducedClass);

        store.SetState(updatedClass);

        actualReducedState.Should().BeEquivalentTo(updatedReducedClass);
    }

    [Fact(DisplayName = "Should reduce state value with type")]
    public void ShouldReduceStateValueWithType()
    {
        var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
        var originalReducedClass = new TestReducer().Execute(originalClass);
        var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
        var updatedReducedClass = new TestReducer().Execute(updatedClass);

        using var serviceProvider =
            serviceCollection.AddTransient<TestReducer>()
            .BuildProviderWithTestLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

        var store = new Store<SimpleClass>(originalClass, serviceProvider);

        SimpleClassSubset actualReducedState = default;

        store.Reduce<TestReducer, SimpleClassSubset>(reducedState =>
        {
            actualReducedState = reducedState;
        });

        actualReducedState.Should().BeEquivalentTo(originalReducedClass);

        store.SetState(updatedClass);

        actualReducedState.Should().BeEquivalentTo(updatedReducedClass);
    }
}
