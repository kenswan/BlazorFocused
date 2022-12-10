// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using Samples.Model;
using ToDoList.Services;
using ToDoList.Stores;

namespace ToDoList.Actions;

public class AddToDoAction : StoreActionAsync<ToDoStore, ToDo>
{
    private readonly IToDoService toDoService;

    public AddToDoAction(IToDoService toDoService)
    {
        this.toDoService = toDoService;
    }

    public override async ValueTask<ToDoStore> ExecuteAsync(ToDo input)
    {
        ToDo newToDo = await toDoService.AddToDoAsync(input);

        State.InComplete.Add(newToDo);

        return State;
    }
}
