using System;
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
        [Fact(DisplayName = "Should execute action async" )]
        public async Task ShouldRetrieveValueAsyncWithInstance()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var testServiceMock = new Mock<TestService>();

            testServiceMock.Setup(service =>
                service.GetValueAsync<SimpleClass>())
                    .ReturnsAsync(updatedClass);

            storeBuilder.RegisterAsyncAction(new TestActionAsync(testServiceMock.Object));

            var store = new Store<SimpleClass>(originalClass, storeBuilder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }

        [Fact(DisplayName = "Should execute action async by type")]
        public async Task ShouldRetrieveValueAsyncWithType()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();

            var testServiceMock = new Mock<TestService>();

            testServiceMock.Setup(service =>
                service.GetValueAsync<SimpleClass>())
                    .ReturnsAsync(updatedClass);

            storeBuilder.RegisterAsyncAction<TestActionAsync>();
            storeBuilder.RegisterService(testServiceMock.Object);

            var store = new Store<SimpleClass>(originalClass, storeBuilder);

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
        }
    }
}
