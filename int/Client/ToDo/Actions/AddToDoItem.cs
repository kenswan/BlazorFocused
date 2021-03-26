using System.Linq;
using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Store;
using Integration.Shared.Models;
using Integration.ToDo.Models;

namespace Integration.ToDo.Actions
{
    public class AddToDoItem : IActionAsync<ToDoStore, ToDoItem>
    {
        public ToDoStore State { get; set; }

        private readonly IRestClient restClient;

        public AddToDoItem(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async ValueTask<ToDoStore> ExecuteAsync(ToDoItem input)
        {
            var toDoItem = await restClient.PostAsync<ToDoItem>("/api/todo", input);

            State.Items = State.Items.Concat(new[] { toDoItem });

            return State;
        }
    }
}
