// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samples.Api;
using Samples.Model;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DbConnection")));

var AllowedCorsSpecification = "_AllowedCorsSpecification";

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowedCorsSpecification, builder =>
    {
        builder.WithOrigins("https://localhost:7272", "http://localhost:5272");
        builder.WithMethods("GET", "POST", "PUT");
        builder.AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(AllowedCorsSpecification);
    app.UseSwagger();
    app.UseSwaggerUI();

    using IServiceScope scope = app.Services.CreateScope();

    ToDoDbContext toDoDbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    if (toDoDbContext.Database.GetPendingMigrations().Any())
    {
        toDoDbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.MapGet("/api/todo", ([FromServices] ToDoDbContext context) =>
{
    return Results.Ok(context.GetToDos());
});

app.MapGet("/api/todo/{id}", async ([FromRoute] Guid id, [FromServices] ToDoDbContext context) =>
{
    return Results.Ok(await context.GetToDoByIdAsync(id));
});

app.MapPost("/api/todo", async ([FromBody] ToDo toDo, [FromServices] ToDoDbContext context) =>
{
    return Results.Ok(await context.InsertToDoAsync(toDo));
});

app.MapPut("/api/todo/{id}", async (
    [FromRoute] Guid id, [FromBody] ToDo toDo, [FromServices] ToDoDbContext context) =>
{
    return id != toDo.Id
        ? Results.BadRequest()
        : context.ToDoExists(id) ? Results.Ok(await context.UpdateToDoAsync(toDo)) : Results.NotFound();
});

app.Run();
