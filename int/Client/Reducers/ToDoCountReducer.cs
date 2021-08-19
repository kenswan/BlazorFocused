using BlazorFocused.Store;
using Integration.Client.Models;
using Integration.Sdk.Models;
using System.Linq;

namespace Integration.Client.Reducers
{
    public class ToDoCountReducer : IReducer<ToDoStore, ToDoCounter>
    {
        public ToDoCounter Execute(ToDoStore input)
        {
            return new ToDoCounter
            {
                Created = input.Items.Where(item => item.Status == ToDoStatus.Created).Count(),
                InProgress = input.Items.Where(item => item.Status == ToDoStatus.InProgress).Count(),
                Completed = input.Items.Where(item => item.Status == ToDoStatus.Complete).Count(),
            };
        }
    }
}
