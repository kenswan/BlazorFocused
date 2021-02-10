using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Integration.Client.Models;
using BlazorFocused.Store;

namespace BlazorFocused.Integration.Client.Actions
{
    public class GetToDoItems : IActionAsync<ToDoStore>
    {
        private readonly IRestClient restClient;

        public GetToDoItems(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async ValueTask<ToDoStore> ExecuteAsync(ToDoStore state)
        {
            var items = await restClient.GetAsync<IEnumerable<ToDoItem>>("api/todo");

            state.Items = items;

            return state;
        }
    }
}
