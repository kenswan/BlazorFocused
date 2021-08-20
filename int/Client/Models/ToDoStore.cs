using Integration.Sdk.Models;
using System.Collections.Generic;
using System.Linq;

namespace Integration.Client.Models
{
    public class ToDoStore
    {
        public IEnumerable<ToDoItem> Items { get; set; }

        public ToDoStore()
        {
            Items = Enumerable.Empty<ToDoItem>();
        }

        public static ToDoStore GetInitialState() =>
            new() { Items = Enumerable.Empty<ToDoItem>() };
    }
}
