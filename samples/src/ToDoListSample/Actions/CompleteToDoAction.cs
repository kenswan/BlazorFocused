using BlazorFocused.Store;
using Samples.Model;
using ToDoListSample.Services;
using ToDoListSample.Stores;

namespace ToDoListSample.Actions
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
