using BlazorFocused.Store;
using ToDoListSample.Services;
using ToDoListSample.Stores;


namespace ToDoListSample.Actions
{
    public class GetAllToDoAction : StoreActionAsync<ToDoStore>
    {
        private readonly IToDoService toDoService;

        public GetAllToDoAction(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync()
        {
            var toDos = await toDoService.GetToDoItemsAsync();

            State.Complete = toDos.Where(toDo => toDo.IsCompleted == true).ToList();
            State.InComplete = toDos.Where(toDo => toDo.IsCompleted == false).ToList();

            return State;
        }
    }
}
