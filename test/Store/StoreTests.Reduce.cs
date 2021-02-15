using BlazorFocused.Core.Test.Model;
using BlazorFocused.Core.Test.Utility;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should reduce state value with instance")]
        public void ShouldReduceStateValueWithInstance()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var originalReducedClass = new TestReducer().Execute(originalClass);
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedReducedClass = new TestReducer().Execute(updatedClass);

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterReducer(new TestReducer());
            });

            SimpleClassSubset actualReducedState = default;

            store.Reduce<SimpleClassSubset>(reducedState =>
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

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterReducer<TestReducer, SimpleClassSubset>();
            });

            SimpleClassSubset actualReducedState = default;

            store.Reduce<SimpleClassSubset>(reducedState =>
            {
                actualReducedState = reducedState;
            });

            actualReducedState.Should().BeEquivalentTo(originalReducedClass);

            store.SetState(updatedClass);

            actualReducedState.Should().BeEquivalentTo(updatedReducedClass);
        }
    }
}
