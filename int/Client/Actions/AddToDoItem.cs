﻿using BlazorFocused.Client;
using BlazorFocused.Store;
using Integration.Client.Models;
using Integration.Sdk.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Integration.Client.Actions
{
    public class AddToDoItem : StoreActionAsync<ToDoStore, ToDoItem>
    {
        private readonly IRestClient restClient;

        public AddToDoItem(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public override async ValueTask<ToDoStore> ExecuteAsync(ToDoItem input)
        {
            var toDoItem = await restClient.PostAsync<ToDoItem>("/api/todo", input);

            State.Items = State.Items.Concat(new[] { toDoItem });

            return State;
        }
    }
}