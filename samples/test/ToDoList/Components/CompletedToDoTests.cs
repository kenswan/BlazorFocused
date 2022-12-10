// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using Bogus;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Samples.Model;
using ToDoList.Stores;
using Xunit;

namespace ToDoList.Components;

public class CompletedToDoTests
{
    private readonly Mock<IStore<ToDoStore>> toDoStoreMock;

    public CompletedToDoTests()
    {
        toDoStoreMock = new();
    }

    [Fact]
    public void ShouldRenderCompletedToDoItems()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => toDoStoreMock.Object);
        List<ToDo> completedItems = GenerateCompletedToDos();
        IEnumerable<string> expectedTitles = completedItems.Select(item => item.Title);
        var store = new ToDoStore { Complete = completedItems };

        toDoStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ToDoStore>>()))
            .Callback((Action<ToDoStore> action) => action(store));

        IRenderedComponent<CompletedToDo> component = context.RenderComponent<CompletedToDo>();
        IRefreshableElementCollection<AngleSharp.Dom.IElement> titleElements = component.FindAll(".todo-item-title");
        IEnumerable<string> actualTitles = titleElements.Select(title => title.TextContent);

        Assert.Equal(expectedTitles.Count(), actualTitles.Count());

        actualTitles.Should().BeEquivalentTo(expectedTitles);
    }

    private static List<ToDo> GenerateCompletedToDos()
    {
        return new Faker<ToDo>()
        .RuleFor(todo => todo.Id, fake => fake.Random.Guid())
        .RuleFor(todo => todo.Title, fake => fake.Commerce.Product())
        .RuleFor(todo => todo.IsCompleted, _ => true)
        .Generate(new Faker().Random.Int(3, 5));
    }
}
