using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using BlazorFocused.Tools.Utility;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute action by instance")]
        public void ShouldExecuteActionByInstance()
        {
            SimpleClass originalClass = default;
            serviceCollection.AddTransient<TestAction>();

            using var store = new Store<SimpleClass>(
                originalClass, 
                serviceCollection.BuildProviderWithMockLogger<Store<SimpleClass>>(testOutputHelper));

            store.GetState().Should().BeNull();

            store.Dispatch<TestAction>();

            store.GetState().Should().NotBeNull();
        }

        [Fact(DisplayName = "Should execute action with input by instance")]
        public void ShouldExecuteActionWithInputByInstance()
        {
            var input = new Faker().Random.String2(10);
            SimpleClass originalClass = default;
            SimpleClass expectedClass = SimpleClassUtilities.GetStaticSimpleClass(input);

            var serviceProvider =
                serviceCollection.AddTransient<TestActionWithInput>()
                .BuildProviderWithMockLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

            using var store = new Store<SimpleClass>(originalClass, serviceProvider);

            store.GetState().Should().BeNull();

            store.Dispatch<TestActionWithInput, string>(input);

            store.GetState().Should().NotBeNull()
                .And.BeEquivalentTo(expectedClass);
        }

        [Fact(DisplayName = "Should execute action by type")]
        public void ShouldExecuteActionWithType()
        {
            SimpleClass originalClass = default;

            using var serviceProvider =
                serviceCollection.AddTransient<TestAction>()
                .BuildProviderWithMockLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

            var store = new Store<SimpleClass>(originalClass, serviceProvider);

            store.GetState().Should().BeNull();

            store.Dispatch<TestAction>();

            store.GetState().Should().NotBeNull();
        }

        [Fact(DisplayName = "Should execute action with input by type")]
        public void ShouldExecuteActionWithInputByType()
        {
            var input = new Faker().Random.String2(10);
            SimpleClass originalClass = default;
            SimpleClass expectedClass = SimpleClassUtilities.GetStaticSimpleClass(input);

            using var serviceProvider =
                serviceCollection.AddTransient<TestActionWithInput>()
                .BuildProviderWithMockLogger<Store<SimpleClass>>(testOutputHelper) as ServiceProvider;

            var store = new Store<SimpleClass>(originalClass, serviceProvider);

            store.GetState().Should().BeNull();

            store.Dispatch<TestActionWithInput, string>(input);

            store.GetState().Should().NotBeNull()
                .And.BeEquivalentTo(expectedClass);
        }
    }
}