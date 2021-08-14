using BlazorFocused.Client;
using BlazorFocused.Store;
using Bogus;
using Bunit;
using FluentAssertions;
using Integration.Shared.Models;
using Integration.Test.Utility;
using Integration.ToDo.Actions;
using Integration.ToDo.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Linq;
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
            var apiToDoItemCount = new Faker().Random.Int(3, 5);
            var apiToDoItems = GenerateToDoItems(apiToDoItemCount);
            var initialState = new ToDoStore { Items = Enumerable.Empty<ToDoItem>() };

            mockRestClient.Setup(client => client.GetAsync<IEnumerable<ToDoItem>>("/api/todo"))
                .ReturnsAsync(apiToDoItems);

            context.Services.AddTransient<GetToDoItems>();
            context.Services.AddTransient(sp => mockRestClient.Object);
            context.Services.AddStore(initialState);

            var component = context.RenderComponent<ClientPage.ToDo>();

            var actualItemElements = component.FindAll(".items");

            Assert.Equal(apiToDoItemCount, actualItemElements.Count);
        }

        [Trait(nameof(Category), nameof(Category.Integration))]
        [Fact(DisplayName = "Should add and render todo item on page")]
        public void ShouldAddToDoItem()
        {
            var apiToDoItemCount = 2;
            var apiToDoItems = GenerateToDoItems(apiToDoItemCount);
            var initialState = new ToDoStore { Items = Enumerable.Empty<ToDoItem>() };
            var newToDoItem = GenerateToDoItem();
            newToDoItem.Status = ToDoStatus.Created;

            mockRestClient.Setup(client => client.GetAsync<IEnumerable<ToDoItem>>("/api/todo"))
                .ReturnsAsync(apiToDoItems);

            mockRestClient.Setup(client => client.PostAsync<ToDoItem>("/api/todo", It.Is<ToDoItem>(item =>
                    item.Title == newToDoItem.Title && item.Description == newToDoItem.Description)))
                .ReturnsAsync(newToDoItem);

            context.Services.AddTransient<AddToDoItem>();
            context.Services.AddTransient<GetToDoItems>();
            context.Services.AddTransient(sp => mockRestClient.Object);
            context.Services.AddStore<ToDoStore>(initialState);

            var component = context.RenderComponent<ClientPage.ToDo>();

            component.Find("#title").Change(newToDoItem.Title);
            component.Find("#description").Change(newToDoItem.Description);
            component.Find("form").Submit();

            var actualItemElements = component.FindAll(".items");

            actualItemElements.Should().HaveCount(apiToDoItemCount + 1)
                .And.Contain(x =>
                    x.InnerHtml.Contains(newToDoItem.Title) &&
                    x.InnerHtml.Contains(newToDoItem.Description));
        }

        public ToDoItem GenerateToDoItem() =>
            GetRandomToDo().Generate();

        public IEnumerable<ToDoItem> GenerateToDoItems(int count) =>
            GetRandomToDo().Generate(count);

        private Faker<ToDoItem> GetRandomToDo() =>
            new Faker<ToDoItem>()
                .RuleFor(todo => todo.Title, faker => faker.Commerce.ProductName())
                .RuleFor(todo => todo.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(todo => todo.Status, faker => faker.PickRandom<ToDoStatus>());
    }
}
