using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Core.Test.Utility;
using FluentAssertions;
using Moq;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute action async", Skip = "WIP" )]
        public async Task ShouldRetrieveValueAsyncWithInstance()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var apiUrl = "api/with/instance";
            var testHttpServiceMock = new Mock<TestHttpService>();

            testHttpServiceMock.Setup(service =>
                service.TestGetValueAsync<SimpleClass>(apiUrl))
                    .ReturnsAsync(updatedClass);

            storeBuilder.RegisterAsyncAction(new TestActionAsync(apiUrl));
            storeBuilder.RegisterHttpClient<ITestHttpService, TestHttpService>();

            var store = new Store<SimpleClass>(originalClass, storeBuilder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }

        [Fact(DisplayName = "Should execute action async by type", Skip = "WIP")]
        public async Task ShouldRetrieveValueAsyncWithType()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();

            var testHttpServiceMock = new Mock<TestHttpService>();

            testHttpServiceMock.Setup(service =>
                service.TestGetValueAsync<SimpleClass>(TestActionAsync.DefaultUrl))
                    .ReturnsAsync(updatedClass);

            storeBuilder.RegisterAsyncAction<TestActionAsync>();
            storeBuilder.RegisterHttpClient<ITestHttpService, TestHttpService>();

            var store = new Store<SimpleClass>(originalClass, storeBuilder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }
    }
}
