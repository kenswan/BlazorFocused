using BlazorFocused.Core.Test.Model;
using BlazorFocused.Core.Test.Utility;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute action by instance")]
        public void ShouldExecuteActionWithInstance()
        {
            SimpleClass originalClass = default;

            storeBuilder.RegisterAction(new TestAction());

            var store = new Store<SimpleClass>(originalClass, storeBuilder);

            store.GetState().Should().BeNull();

            store.Dispatch<TestAction>();

            store.GetState().Should().NotBeNull();
        }

        [Fact(DisplayName = "Should execute action by type")]
        public void ShouldExecuteActionWithType()
        {
            SimpleClass originalClass = default;

            storeBuilder.RegisterAction<TestAction>();

            var store = new Store<SimpleClass>(originalClass, storeBuilder);

            store.GetState().Should().BeNull();

            store.Dispatch<TestAction>();

            store.GetState().Should().NotBeNull();
        }
    }
}