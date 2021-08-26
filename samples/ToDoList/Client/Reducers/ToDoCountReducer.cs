using BlazorFocused.Store;
using ToDoList.Client.Models;
using ToDoList.Client.Stores;

namespace ToDoList.Client.Reducers
{
    public class ToDoCountReducer : IReducer<ToDoStore, ToDoCount>
    {
        public ToDoCount Execute(ToDoStore store)
        {
            var complete = store.Complete.Count;
            var incomplete = store.InComplete.Count;

            return new ToDoCount
            {
                Complete = complete,
                InComplete = incomplete,
                Total = complete + incomplete
            };
        }
    }
}
