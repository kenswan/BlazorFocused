using BlazorFocused.Store;
using ToDoList.Models;
using ToDoList.Stores;

namespace ToDoList.Reducers
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
