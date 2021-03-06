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

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
