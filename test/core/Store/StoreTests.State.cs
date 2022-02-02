using BlazorFocused.Tools.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should update state")]
        public void ShouldUpdateState()
        {
            var originalState = new SimpleClass { FieldOne = "Original" };
            var expectedState = new SimpleClass { FieldOne = "Expected" };
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var store = new Store<SimpleClass>(originalState, serviceProvider);

            store.SetState(expectedState);

            var actualState = store.GetState();

            actualState.Should().BeEquivalentTo(expectedState);
        }

        [Fact(DisplayName = "Should update state with method")]
        public void ShouldUpdateStateWithMethod()
        {
            var originalState = new SimpleClass { FieldOne = "Original" };
            var expectedState = new SimpleClass { FieldOne = "Expected" };
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var store = new Store<SimpleClass>(originalState, serviceProvider);

            store.SetState(currentState =>
            {
                currentState.FieldOne = "Expected";
                return currentState;
            });

            var actualState = store.GetState();

            actualState.Should().BeEquivalentTo(expectedState);
        }

        [Fact(DisplayName = "Should subscribe to state")]
        public void ShouldSubscribeState()
        {
            var originalState = new SimpleClass { FieldOne = "Original" };
            var expectedState = new SimpleClass { FieldOne = "Expected" };
            SimpleClass updatedState = null;
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var store = new Store<SimpleClass>(originalState, serviceProvider);

            store.Subscribe((newState) => { updatedState = newState; });

            store.GetState().Should().BeEquivalentTo(originalState);

            store.SetState(expectedState);

            updatedState.Should().BeEquivalentTo(expectedState);
        }
    }
}
