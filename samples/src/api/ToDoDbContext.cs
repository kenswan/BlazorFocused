// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Samples.Model;

namespace Samples.Api;

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

        ToDo toDo = ToDos.Where(item => item.Id == id).FirstOrDefault();

        return toDo is not null;
    }

    public async Task<ToDo> GetToDoByIdAsync(Guid id)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        return await ToDos.FindAsync(id);
    }

    public async Task<ToDo> InsertToDoAsync(ToDo toDo)
    {
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ToDo> entityEntry = await ToDos.AddAsync(toDo);
        await SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<ToDo> UpdateToDoAsync(ToDo toDo)
    {
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ToDo> entityEntry = ToDos.Update(toDo);
        await SaveChangesAsync();

        return entityEntry.Entity;
    }
}
