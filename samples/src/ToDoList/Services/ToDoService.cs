// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using Samples.Model;

namespace ToDoList.Services;

public class ToDoService : IToDoService
{
    private readonly IRestClient restClient;

    public ToDoService(IRestClient restClient)
    {
        this.restClient = restClient;
    }

    public async Task<ToDo> AddToDoAsync(ToDo toDo)
    {
        return await restClient.PostAsync<ToDo>("api/todo", toDo);
    }

    public async Task<ToDo> CompleteToDoAsync(ToDo toDo)
    {
        toDo.IsCompleted = true;

        return await restClient.PutAsync<ToDo>($"api/todo/{toDo.Id}", toDo);
    }

    public async Task<IEnumerable<ToDo>> GetToDoItemsAsync()
    {
        return await restClient.GetAsync<IEnumerable<ToDo>>("api/todo");
    }

    public async Task<ToDo> RestoreToDoAsync(ToDo toDo)
    {
        toDo.IsCompleted = false;

        return await restClient.PutAsync<ToDo>($"api/todo/{toDo.Id}", toDo);
    }
}
