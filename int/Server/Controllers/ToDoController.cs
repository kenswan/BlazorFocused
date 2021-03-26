using System.Collections.Generic;
using System.Threading.Tasks;
using Integration.Server.Services;
using Integration.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Server.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ToDoController : ControllerBase
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

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> Post([FromBody] ToDoItem toDoItem)
        {
            return await toDoService.AddToDoItem(toDoItem);
        }
    }
}
