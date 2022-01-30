using BlazorFocused.Store;
using ToDo.Client.Services;
using ToDo.Client.Stores;
using Model = ToDo.Shared;

namespace ToDo.Client.Actions
{
    public class RestoreToDoAction : StoreActionAsync<ToDoStore, Model.ToDo>
    {
        private readonly IToDoService toDoService;

        public RestoreToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(Model.ToDo toDo)
        {
            var restoredToDo = await toDoService.RestoreToDoAsync(toDo);

            State.InComplete.Add(restoredToDo);
            State.Complete.Remove(toDo);

            return State;
        }
    }
}
