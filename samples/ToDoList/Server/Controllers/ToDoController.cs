using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Server.Providers;
using ToDoList.Shared;

namespace ToDoList.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext toDoDbContext;

        public ToDoController(ToDoDbContext toDoDbContext)
        {
            this.toDoDbContext = toDoDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> Get()
        {
            return Ok(toDoDbContext.GetToDos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> Get(Guid id)
        {
            return Ok(await toDoDbContext.GetToDoByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Post([FromBody] ToDo toDo)
        {
            return Ok(await toDoDbContext.InsertToDoAsync(toDo));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> Put(Guid id, [FromBody] ToDo toDo)
        {
            if (id != toDo.Id)
            {
                return BadRequest();
            }

            if (toDoDbContext.ToDoExists(id))
            {
                return await toDoDbContext.UpdateToDoAsync(toDo);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
