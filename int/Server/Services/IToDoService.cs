using System.Collections.Generic;
using Integration.Shared.Models;

namespace Integration.Server.Services
{
    public interface IToDoService
    {
        IEnumerable<ToDoItem> GenerateToDoItems(int count);
    }
}
