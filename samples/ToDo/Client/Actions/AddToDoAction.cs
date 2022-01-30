using BlazorFocused.Store;
using ToDo.Client.Services;
using ToDo.Client.Stores;
using Model = ToDo.Shared;

namespace ToDo.Client.Actions
{
    public class AddToDoAction : StoreActionAsync<ToDoStore, Model.ToDo>
    {
        private readonly IToDoService toDoService;

        public AddToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(Model.ToDo toDo)
        {
            var newToDo = await toDoService.AddToDoAsync(toDo);

            State.InComplete.Add(newToDo);

            return State;
        }
    }
}
