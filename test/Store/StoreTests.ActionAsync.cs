using System.Threading.Tasks;
using BlazorFocused.Test.Model;
using BlazorFocused.Test.Utility;
using Bogus;
using FluentAssertions;
using Moq;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute async action by instance")]
        public async Task ShouldRetrieveValueAsyncByInstance()
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

        [Fact(DisplayName = "Should execute async action with input by instance")]
        public async Task ShouldRetrieveValueAsyncWithInputByInstance()
        {
            var input = new Faker().Random.String();
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var testServiceMock = new Mock<TestService>();

            testServiceMock.Setup(service =>
                service.GetValueAsync<string, SimpleClass>(input))
                    .ReturnsAsync(updatedClass);

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction(new TestActionAsyncWithInput(testServiceMock.Object));
            });

            await store.DispatchAsync<TestActionAsyncWithInput, string>(input);

            store.GetState().Should().BeEquivalentTo(updatedClass);

            testServiceMock.Verify(service => 
                service.GetValueAsync<string, SimpleClass>(input), Times.Once);
        }

        [Fact(DisplayName = "Should execute async action by type")]
        public async Task ShouldRetrieveValueAsyncByType()
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

        [Fact(DisplayName = "Should execute async action with input by type")]
        public async Task ShouldRetrieveValueAsyncWithInputByType()
        {
            var input = new Faker().Random.String();
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var testServiceMock = new Mock<TestService>();

            testServiceMock.Setup(service =>
                service.GetValueAsync<string, SimpleClass>(input))
                    .ReturnsAsync(updatedClass);

            var store = new Store<SimpleClass>(builder =>
            {
                builder.SetInitialState(originalClass);
                builder.RegisterAction<TestActionAsyncWithInput>();
                builder.RegisterService(testServiceMock.Object);
            });

            await store.DispatchAsync<TestActionAsyncWithInput, string>(input);

            store.GetState().Should().BeEquivalentTo(updatedClass);

            testServiceMock.Verify(service => 
                service.GetValueAsync<string, SimpleClass>(input), Times.Once);
        }
    }
}
