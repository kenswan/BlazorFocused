﻿using BlazorFocused.Store;
using Integration.Client.Actions;
using Integration.Client.Models;
using Integration.Sdk.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integration.Client.Pages
{
    public partial class ToDo : ComponentBase
    {
        [Inject]
        protected IStore<ToDoStore> todoStore { get; set; }

        private IEnumerable<ToDoItem> toDoItems { get; set; } = Enumerable.Empty<ToDoItem>();

        private ToDoItem newToDoItem = InitializeToDoItem();

        protected override async Task OnInitializedAsync()
        {
            await todoStore.DispatchAsync<GetToDoItems>();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                todoStore.Subscribe(state =>
                {
                    toDoItems = state.Items;

                    StateHasChanged();
                });
            }
        }

        private async Task AddToDoItemAsync()
        {
            await todoStore.DispatchAsync<AddToDoItem, ToDoItem>(newToDoItem);

            newToDoItem = InitializeToDoItem();
        }

        private static ToDoItem InitializeToDoItem() =>
            new() { Status = ToDoStatus.Created };
    }
}
