using BlazorFocused.Store;
using ToDoListSample.Services;
using ToDoListSample.Stores;
using Samples.Model;

namespace ToDoListSample.Actions
{
    public class RestoreToDoAction : StoreActionAsync<ToDoStore, ToDo>
    {
        private readonly IToDoService toDoService;

        public RestoreToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(ToDo toDo)
        {
            var restoredToDo = await toDoService.RestoreToDoAsync(toDo);

            State.InComplete.Add(restoredToDo);
            State.Complete.Remove(toDo);

            return State;
        }
    }
}
