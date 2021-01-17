using System;
using System.Linq;
using BlazorFocused.Integration.Client.Models;
using BlazorFocused.Store;

namespace BlazorFocused.Integration.Client.Reducers
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
