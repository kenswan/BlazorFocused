using System.Threading.Tasks;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Core.Test.Utility;
using FluentAssertions;
using Moq;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute action async")]
        public async Task ShouldRetrieveValueAsyncWithInstance()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var apiUrl = "api/with/instance";

            restClientMock.Setup(client =>
                client.GetAsync<SimpleClass>(apiUrl))
                    .ReturnsAsync(updatedClass);

            var builder = new StoreBuilder<SimpleClass>();
            builder.RegisterAsyncAction(new TestActionAsync(apiUrl));

            var store = new Store<SimpleClass>(originalClass, restClientMock.Object);
            store.LoadBuilder(builder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }

        [Fact(DisplayName = "Should execute action async by type")]
        public async Task ShouldRetrieveValueAsyncWithType()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var apiUrl = "api/with/type";

            restClientMock.Setup(client =>
                client.GetAsync<SimpleClass>(apiUrl))
                    .ReturnsAsync(updatedClass);

            var builder = new StoreBuilder<SimpleClass>();
            builder.RegisterAsyncAction<TestActionAsync>();

            var store = new Store<SimpleClass>(originalClass, restClientMock.Object);
            store.LoadBuilder(builder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }
    }
}
