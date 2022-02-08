using BlazorFocused.Store;
using Samples.Model;
using ToDoList.Services;
using ToDoList.Stores;

namespace ToDoList.Actions
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
