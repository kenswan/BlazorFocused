using Integration.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integration.Server.Services
{
    public interface IToDoService
    {
        ValueTask<ToDoItem> AddToDoItem(ToDoItem toDoItem);

        IEnumerable<ToDoItem> GenerateToDoItems(int count);
    }
}
