using BlazorFocused.Client;
using BlazorFocused.Core.Test.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreTests
    {
        private readonly Mock<IRestClient> restClientMock;

        public StoreTests()
        {
            restClientMock = new Mock<IRestClient>();
        }

        [Fact(DisplayName = "Should Store and Return Initial Value")]
        public void ShouldStoreAndReturnInitialValue()
        {
            SimpleClass inputSimpleClass = SimpleClassUtilities.GetRandomSimpleClass();
            SimpleClass expectedSimpleClass = inputSimpleClass;

            var store = new Store<SimpleClass>(inputSimpleClass, restClientMock.Object);

            SimpleClass actualSimpleClass = store.GetState();

            actualSimpleClass.Should().BeEquivalentTo(expectedSimpleClass);
        }

        [Fact(DisplayName = "Should Return 'null' when initialized as null")]
        public void ShouldReturnNullWhenInitializedAsNull()
        {
            var store = new Store<SimpleClass>(null, restClientMock.Object);

            SimpleClass actualSimpleClass = store.GetState();

            actualSimpleClass.Should().BeNull();
        }
    }
}
