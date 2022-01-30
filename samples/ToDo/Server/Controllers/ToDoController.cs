using Microsoft.AspNetCore.Mvc;
using ToDo.Server.Providers;
using Model = ToDo.Shared;

namespace ToDo.Server.Controllers
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
        public ActionResult<IEnumerable<Model.ToDo>> Get()
        {
            return Ok(toDoDbContext.GetToDos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model.ToDo>> Get(Guid id)
        {
            return Ok(await toDoDbContext.GetToDoByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<Model.ToDo>> Post([FromBody] Model.ToDo toDo)
        {
            return Ok(await toDoDbContext.InsertToDoAsync(toDo));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Model.ToDo>> Put(Guid id, [FromBody] Model.ToDo toDo)
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
