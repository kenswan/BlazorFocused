using BlazorFocused.Store;
using ToDo.Client.Services;
using ToDo.Client.Stores;
using Model = ToDo.Shared;

namespace ToDo.Client.Actions
{
    public class CompleteToDoAction : StoreActionAsync<ToDoStore, Model.ToDo>
    {
        private readonly IToDoService toDoService;

        public CompleteToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(Model.ToDo toDo)
        {
            var completedToDo = await toDoService.CompleteToDoAsync(toDo);

            State.Complete.Add(completedToDo);
            State.InComplete.Remove(toDo);

            return State;
        }
    }
}
