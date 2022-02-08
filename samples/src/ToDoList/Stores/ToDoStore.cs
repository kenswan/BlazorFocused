using Samples.Model;

namespace ToDoList.Stores
{
    public class ToDoStore
    {
        public List<ToDo> InComplete { get; set; } = new();

        public List<ToDo> Complete { get; set; } = new();
    }
}
