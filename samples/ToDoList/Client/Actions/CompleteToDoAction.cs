using BlazorFocused.Store;
using System.Threading.Tasks;
using ToDoList.Client.Services;
using ToDoList.Client.Stores;
using ToDoList.Shared;

namespace ToDoList.Client.Actions
{
    public class CompleteToDoAction : StoreActionAsync<ToDoStore, ToDo>
    {
        private readonly IToDoService toDoService;

        public CompleteToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(ToDo toDo)
        {
            var completedToDo = await toDoService.CompleteToDoAsync(toDo);

            State.Complete.Add(completedToDo);
            State.InComplete.Remove(toDo);

            return State;
        }
    }
}
