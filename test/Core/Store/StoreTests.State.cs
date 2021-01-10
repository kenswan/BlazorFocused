using BlazoRx.Core.Store;
using BlazoRx.Core.Test.Model;
using FluentAssertions;
using Xunit;

namespace BlazoRx.Core.Test.Store
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should update state")]
        public void ShouldUpdateState()
        {
            var originalState = new SimpleClass { FieldOne = "Original" };
            var expectedState = new SimpleClass { FieldOne = "Expected" };

            var store = new Store<SimpleClass>(originalState);

            store.SetState(expectedState);

            var actualState = store.GetState();

            actualState.Should().BeEquivalentTo(expectedState);
        }

        [Fact(DisplayName = "Should update state with method")]
        public void ShouldUpdateStateWithMethod()
        {
            var originalState = new SimpleClass { FieldOne = "Original" };
            var expectedState = new SimpleClass { FieldOne = "Expected" };

            var store = new Store<SimpleClass>(originalState);

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

            var store = new Store<SimpleClass>(originalState);

            store.Subscribe((newState) => { updatedState = newState; });

            store.GetState().Should().BeEquivalentTo(originalState);

            store.SetState(expectedState);

            updatedState.Should().BeEquivalentTo(expectedState);
        }
    }
}
