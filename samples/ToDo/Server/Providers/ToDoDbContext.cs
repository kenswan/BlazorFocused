using Microsoft.EntityFrameworkCore;
using Model = ToDo.Shared;

namespace ToDo.Server.Providers
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<Model.ToDo> ToDos { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options) { }

        public IEnumerable<Model.ToDo> GetToDos()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return ToDos;
        }

        public bool ToDoExists(Guid id)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var toDo = ToDos.Where(item => item.Id == id).FirstOrDefault();

            return toDo is not null;
        }

        public async Task<Model.ToDo> GetToDoByIdAsync(Guid id)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ToDos.FindAsync(id);
        }

        public async Task<Model.ToDo> InsertToDoAsync(Model.ToDo toDo)
        {
            var entityEntry = await ToDos.AddAsync(toDo);
            await SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async Task<Model.ToDo> UpdateToDoAsync(Model.ToDo toDo)
        {
            var entityEntry = ToDos.Update(toDo);
            await SaveChangesAsync();

            return entityEntry.Entity;
        }
    }
}
