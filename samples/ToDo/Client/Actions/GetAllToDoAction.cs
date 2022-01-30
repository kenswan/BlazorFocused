using BlazorFocused.Store;
using ToDo.Client.Services;
using ToDo.Client.Stores;


namespace ToDo.Client.Actions
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
