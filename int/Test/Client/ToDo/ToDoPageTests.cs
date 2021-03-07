using System.Collections.Generic;
using System.Linq;
using BlazorFocused.Client;
using BlazorFocused.Store;
using Bogus;
using Bunit;
using Integration.Shared.Models;
using Integration.Test.Utility;
using Integration.ToDo.Actions;
using Integration.ToDo.Models;
using Moq;
using Xunit;
using ClientPage = Integration.ToDo.Pages;

namespace Integration.Test.Client.ToDo
{
    public class ToDoPageTests
    {
        private readonly Mock<IRestClient> mockRestClient;
        private readonly TestContext context;

        public ToDoPageTests()
        {
            context = new TestContext();

            mockRestClient = new Mock<IRestClient>();
        }

        [Trait(nameof(Category), nameof(Category.Integration))]
        [Fact(DisplayName = "Should render todo items on page")]
        public void ShouldRenderToDoItems()
        {
            var apiToDoItemCount = 5;
            var apiToDoItems = GenerateToDoItems(apiToDoItemCount);
            var initialState = new ToDoStore { Items = Enumerable.Empty<ToDoItem>() };

            mockRestClient.Setup(client => client.GetAsync<IEnumerable<ToDoItem>>("/api/todo"))
                .ReturnsAsync(apiToDoItems);

            context.Services.AddStore<ToDoStore>(builder =>
            {
                builder.RegisterAsyncAction<GetToDoItems>();
                builder.RegisterService<IRestClient>(mockRestClient.Object);
                builder.SetInitialState(initialState);
            });

            var component = context.RenderComponent<ClientPage.ToDo>();

            var actualItemElements = component.FindAll("p");

            Assert.Equal(apiToDoItemCount, actualItemElements.Count);
        }

        public IEnumerable<ToDoItem> GenerateToDoItems(int count) =>
            GetRandomToDo().Generate(count);

        private Faker<ToDoItem> GetRandomToDo() =>
            new Faker<ToDoItem>()
                .RuleFor(todo => todo.Title, faker => faker.Commerce.ProductName())
                .RuleFor(todo => todo.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(todo => todo.Status, faker => faker.PickRandom<ToDoStatus>());
    }
}
