using BlazorFocused.Tools.Model;
using BlazorFocused.Tools.Utility;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BlazorFocused.Store
{
    public partial class StoreTests
    {
        [Fact(DisplayName = "Should execute async action")]
        public async Task ShouldRetrieveValueAsyncByInstance()
        {
            var originalClass = SimpleClassUtilities.GetRandomSimpleClass();
            var updatedClass = SimpleClassUtilities.GetRandomSimpleClass();
            var testServiceMock = new Mock<TestService>();

            testServiceMock.Setup(service =>
                service.GetValueAsync<SimpleClass>())
                    .ReturnsAsync(updatedClass);

            var serviceProvider = serviceCollection
                .AddTransient<TestActionAsync>()
                .AddTransient(sp => testServiceMock.Object)
                .BuildServiceProvider();

            var store = new Store<SimpleClass>(originalClass, serviceProvider);

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

            var serviceProvider = serviceCollection
                .AddTransient<TestActionAsyncWithInput>()
                .AddTransient(sp => testServiceMock.Object)
                .BuildServiceProvider();

            var store = new Store<SimpleClass>(originalClass, serviceProvider);

            await store.DispatchAsync<TestActionAsyncWithInput, string>(input);

            store.GetState().Should().BeEquivalentTo(updatedClass);

            testServiceMock.Verify(service =>
                service.GetValueAsync<string, SimpleClass>(input), Times.Once);
        }
    }
}
