using System.Collections.Generic;
using System.Linq;
using Integration.Shared.Models;

namespace Integration.ToDo.Models
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
