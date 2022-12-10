// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Samples.Model;

namespace ToDoList.Services;

public interface IToDoService
{
    public Task<ToDo> AddToDoAsync(ToDo toDo);

    public Task<IEnumerable<ToDo>> GetToDoItemsAsync();

    public Task<ToDo> CompleteToDoAsync(ToDo toDo);

    public Task<ToDo> RestoreToDoAsync(ToDo toDo);
}
