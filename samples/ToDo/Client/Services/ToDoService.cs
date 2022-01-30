using BlazorFocused.Client;
using Model = ToDo.Shared;

namespace ToDo.Client.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IRestClient restClient;

        public ToDoService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<Model.ToDo> AddToDoAsync(Model.ToDo toDo)
        {
            return await restClient.PostAsync<Model.ToDo>("api/todo", toDo);
        }

        public async Task<Model.ToDo> CompleteToDoAsync(Model.ToDo toDo)
        {
            toDo.IsCompleted = true;

            return await restClient.PutAsync<Model.ToDo>($"api/todo/{toDo.Id}", toDo);
        }

        public async Task<IEnumerable<Model.ToDo>> GetToDoItemsAsync()
        {
            return await restClient.GetAsync<IEnumerable<Model.ToDo>>("api/todo");
        }

        public async Task<Model.ToDo> RestoreToDoAsync(Model.ToDo toDo)
        {
            toDo.IsCompleted = false;

            return await restClient.PutAsync<Model.ToDo>($"api/todo/{toDo.Id}", toDo);
        }
    }
}
