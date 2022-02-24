using BlazorFocused;
using ToDoList.Services;
using ToDoList.Stores;
using Samples.Model;

namespace ToDoList.Actions
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
