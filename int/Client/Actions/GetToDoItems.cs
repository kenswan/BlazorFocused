using BlazorFocused.Client;
using BlazorFocused.Store;
using Integration.Client.Models;
using Integration.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integration.Client.Actions
{
    public class GetToDoItems : StoreActionAsync<ToDoStore>
    {
        private readonly IRestClient restClient;

        public GetToDoItems(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync()
        {
            var items = await restClient.GetAsync<IEnumerable<ToDoItem>>("/api/todo");

            State.Items = items;

            return State;
        }
    }
}
