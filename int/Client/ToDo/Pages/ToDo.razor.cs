using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorFocused.Store;
using Integration.Shared.Models;
using Integration.ToDo.Actions;
using Integration.ToDo.Models;
using Microsoft.AspNetCore.Components;

namespace Integration.ToDo.Pages
{
    public partial class ToDo : ComponentBase
    {
        [Inject]
        protected IStore<ToDoStore> todoStore { get; set; }

        private IEnumerable<ToDoItem> toDoItems { get; set; } = Enumerable.Empty<ToDoItem>();

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
    }
}
