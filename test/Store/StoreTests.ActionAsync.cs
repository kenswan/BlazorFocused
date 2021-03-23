using System.Threading.Tasks;
using BlazorFocused.Test.Model;
using BlazorFocused.Test.Utility;
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
            var testServiceMock = new Mock<TestService>();

            testServiceMock.Setup(service =>
                service.GetValueAsync<SimpleClass>())
                    .ReturnsAsync(updatedClass);

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction(new TestActionAsync(testServiceMock.Object));
            });

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
            testServiceMock.Verify(service => service.GetValueAsync<SimpleClass>(), Times.Once);
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

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction<TestActionAsync>();
                builder.RegisterService(testServiceMock.Object);
            });

            await store.DispatchAsync<TestActionAsync>();

            store.GetState().Should().BeEquivalentTo(updatedClass);
            testServiceMock.Verify(service => service.GetValueAsync<SimpleClass>(), Times.Once);
        }
    }
}
