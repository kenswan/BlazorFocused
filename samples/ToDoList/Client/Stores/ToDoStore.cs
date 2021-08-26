using System.Collections.Generic;
using ToDoList.Shared;

namespace ToDoList.Client.Stores
{
    public class ToDoStore
    {
        public List<ToDo> InComplete { get; set; } = new();

        public List<ToDo> Complete { get; set; } = new();
    }
}
