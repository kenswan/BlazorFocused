using Model = ToDo.Shared;

namespace ToDo.Client.Stores
{
    public class ToDoStore
    {
        public List<Model.ToDo> InComplete { get; set; } = new();

        public List<Model.ToDo> Complete { get; set; } = new();
    }
}
