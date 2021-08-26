using BlazorFocused.Store;
using System.Threading.Tasks;
using ToDoList.Client.Services;
using ToDoList.Client.Stores;
using ToDoList.Shared;

namespace ToDoList.Client.Actions
{
    public class AddToDoAction : StoreActionAsync<ToDoStore, ToDo>
    {
        private readonly IToDoService toDoService;

        public AddToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(ToDo toDo)
        {
            var newToDo = await toDoService.AddToDoAsync(toDo);

            State.InComplete.Add(newToDo);

            return State;
        }
    }
}
