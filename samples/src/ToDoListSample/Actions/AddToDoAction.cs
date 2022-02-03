using BlazorFocused.Store;
using Samples.Model;
using ToDoListSample.Services;
using ToDoListSample.Stores;

namespace ToDoListSample.Actions
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
