using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Shared;

namespace ToDoList.Server.Providers
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options) { }

        public IEnumerable<ToDo> GetToDos()
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

        public async Task<ToDo> GetToDoByIdAsync(Guid id)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await ToDos.FindAsync(id);
        }

        public async Task<ToDo> InsertToDoAsync(ToDo toDo)
        {
            var entityEntry = await ToDos.AddAsync(toDo);
            await SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async Task<ToDo> UpdateToDoAsync(ToDo toDo)
        {
            var entityEntry = ToDos.Update(toDo);
            await SaveChangesAsync();

            return entityEntry.Entity;
        }
    }
}
