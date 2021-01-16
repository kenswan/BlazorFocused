using System.Threading.Tasks;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Core.Test.Utility;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute action async")]
        public async Task ShouldRetrieveValueAsync()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var apiUrl = "api/test";

            restClientMock.Setup(client =>
                client.GetAsync<SimpleClass>(It.IsAny<string>()))
                    .ReturnsAsync(updatedClass);

            var builder = new StoreBuilder<SimpleClass>();
            builder.RegisterAsyncAction(new TestActionAsync());
            var store = new Store<SimpleClass>(originalClass, restClientMock.Object);
            store.LoadBuilder(builder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }
    }
}
