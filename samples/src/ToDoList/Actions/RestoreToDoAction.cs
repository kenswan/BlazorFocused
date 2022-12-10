// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using Samples.Model;
using ToDoList.Services;
using ToDoList.Stores;

namespace ToDoList.Actions;

public class RestoreToDoAction : StoreActionAsync<ToDoStore, ToDo>
{
    private readonly IToDoService toDoService;

    public RestoreToDoAction(IToDoService toDoService)
    {
        this.toDoService = toDoService;
    }

    public override async ValueTask<ToDoStore> ExecuteAsync(ToDo input)
    {
        ToDo restoredToDo = await toDoService.RestoreToDoAsync(input);

        State.InComplete.Add(restoredToDo);
        State.Complete.Remove(input);

        return State;
    }
}
