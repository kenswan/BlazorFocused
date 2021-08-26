using BlazorFocused.Store;
using System.Threading.Tasks;
using ToDoList.Client.Services;
using ToDoList.Client.Stores;
using ToDoList.Shared;

namespace ToDoList.Client.Actions
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
