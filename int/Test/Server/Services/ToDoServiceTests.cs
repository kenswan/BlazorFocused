using Bogus;
using FluentAssertions;
using Integration.Utility;
using Xunit;

namespace Integration.Server.Services
{
    public class ToDoServiceTests
    {
        private readonly IToDoService toDoService;

        public ToDoServiceTests()
        {
            toDoService = new ToDoService();
        }

        [Trait(nameof(Category), nameof(Category.Unit))]
        [Fact(DisplayName = "Should generate random todo items")]
        public void ShouldGenerateRandomToDoList()
        {
            int itemCount = new Faker().Random.Int(5, 10);

            var toDoItems = toDoService.GenerateToDoItems(itemCount);

            toDoItems.Should().NotBeNull()
                .And.HaveCount(itemCount);
        }
    }
}
