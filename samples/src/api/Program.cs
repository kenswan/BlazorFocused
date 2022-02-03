using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Samples.Api;
using Samples.Model;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(AllowedCorsSpecification);
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();

    var toDoDbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    if (toDoDbContext.Database.GetPendingMigrations().Any())
    {
        toDoDbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.MapGet("/api/todo", ([FromServices]ToDoDbContext context) =>
{
    return Results.Ok(context.GetToDos());
});

app.MapGet("/api/todo/{id}", async ([FromRoute]Guid id, [FromServices]ToDoDbContext context) =>
{
    return Results.Ok(await context.GetToDoByIdAsync(id));
});

app.MapPost("/api/todo", async ([FromBody]ToDo toDo, [FromServices]ToDoDbContext context) =>
{
    return Results.Ok(await context.InsertToDoAsync(toDo));
});

app.MapPut("/api/todo/{id}", async (
    [FromRoute] Guid id, [FromBody] ToDo toDo, [FromServices] ToDoDbContext context) =>
{
    if (id != toDo.Id)
        return Results.BadRequest();

    if (context.ToDoExists(id))
    {
        return Results.Ok(await context.UpdateToDoAsync(toDo));
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();
