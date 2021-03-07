using System.Collections.Generic;
using Integration.Server.Services;
using Integration.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Server.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        public const int INITIAL_TODO_COUNT = 10;

        private readonly IToDoService toDoService;

        public ToDoController(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        [HttpGet]
        public IEnumerable<ToDoItem> Get()
        {
            return toDoService.GenerateToDoItems(10);
        }
    }
}
