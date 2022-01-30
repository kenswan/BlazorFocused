using Model = ToDo.Shared;

namespace ToDo.Client.Services
{
    public interface IToDoService
    {
        public Task<Model.ToDo> AddToDoAsync(Model.ToDo toDo);

        public Task<IEnumerable<Model.ToDo>> GetToDoItemsAsync();

        public Task<Model.ToDo> CompleteToDoAsync(Model.ToDo toDo);

        public Task<Model.ToDo> RestoreToDoAsync(Model.ToDo toDo);
    }
}
