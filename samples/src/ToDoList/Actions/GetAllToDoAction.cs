// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using ToDoList.Services;
using ToDoList.Stores;

namespace ToDoList.Actions;

public class GetAllToDoAction : StoreActionAsync<ToDoStore>
{
    private readonly IToDoService toDoService;

    public GetAllToDoAction(IToDoService toDoService)
    {
        this.toDoService = toDoService;
    }

    public override async ValueTask<ToDoStore> ExecuteAsync()
    {
        IEnumerable<Samples.Model.ToDo> toDos = await toDoService.GetToDoItemsAsync();

        State.Complete = toDos.Where(toDo => toDo.IsCompleted == true).ToList();
        State.InComplete = toDos.Where(toDo => toDo.IsCompleted == false).ToList();

        return State;
    }
}
