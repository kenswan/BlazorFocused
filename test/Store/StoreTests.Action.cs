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
            var serviceCollection = new ServiceCollection();
            SimpleClass originalClass = default;

            serviceCollection.AddStore(originalClass, builder =>
            {
                builder.RegisterAction(new TestAction());
            });

            var provider = serviceCollection.BuildServiceProvider();
            var store = provider.GetRequiredService<IStore<SimpleClass>>();

            store.GetState().Should().BeNull();

            store.Dispatch<TestAction>();

            store.GetState().Should().NotBeNull();
        }

        [Fact(DisplayName = "Should execute action by type")]
        public void ShouldExecuteActionWithType()
        {
            var serviceCollection = new ServiceCollection();
            SimpleClass originalClass = default;

            serviceCollection.AddStore(originalClass, builder =>
            {
                builder.RegisterAction<TestAction>();
            });

            var provider = serviceCollection.BuildServiceProvider();
            var store = provider.GetRequiredService<IStore<SimpleClass>>();

            store.GetState().Should().BeNull();

            store.Dispatch<TestAction>();

            store.GetState().Should().NotBeNull();
        }
    }
}