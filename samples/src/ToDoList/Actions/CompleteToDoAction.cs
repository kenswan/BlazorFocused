// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using Samples.Model;
using ToDoList.Services;
using ToDoList.Stores;

namespace ToDoList.Actions;

public class CompleteToDoAction : StoreActionAsync<ToDoStore, ToDo>
{
    private readonly IToDoService toDoService;

    public CompleteToDoAction(IToDoService toDoService)
    {
        this.toDoService = toDoService;
    }

    public override async ValueTask<ToDoStore> ExecuteAsync(ToDo input)
    {
        ToDo completedToDo = await toDoService.CompleteToDoAsync(input);

        State.Complete.Add(completedToDo);
        State.InComplete.Remove(input);

        return State;
    }
}
