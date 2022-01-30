using BlazorFocused.Store;
using ToDo.Client.Models;
using ToDo.Client.Stores;

namespace ToDo.Client.Reducers
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
