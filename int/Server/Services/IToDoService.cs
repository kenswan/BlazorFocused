using System.Collections.Generic;
using System.Threading.Tasks;
using Integration.Shared.Models;

namespace Integration.Server.Services
{
    public interface IToDoService
    {
        ValueTask<ToDoItem> AddToDoItem(ToDoItem toDoItem);

        IEnumerable<ToDoItem> GenerateToDoItems(int count);
    }
}
