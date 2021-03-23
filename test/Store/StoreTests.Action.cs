using BlazorFocused.Test.Model;
using BlazorFocused.Test.Utility;
using Bogus;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute action by instance")]
        public void ShouldExecuteActionByInstance()
        {
            SimpleClass originalClass = default;

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction(new TestAction());
            });

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

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction(new TestActionWithInput());
            });

            store.GetState().Should().BeNull();

            store.Dispatch<TestActionWithInput, string>(input);

            store.GetState().Should().NotBeNull()
                .And.BeEquivalentTo(expectedClass);
        }

        [Fact(DisplayName = "Should execute action by type")]
        public void ShouldExecuteActionWithType()
        {
            SimpleClass originalClass = default;

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction<TestAction>();
            });

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

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction<TestActionWithInput>();
            });

            store.GetState().Should().BeNull();

            store.Dispatch<TestActionWithInput, string>(input);

            store.GetState().Should().NotBeNull()
                .And.BeEquivalentTo(expectedClass);
        }
    }
}