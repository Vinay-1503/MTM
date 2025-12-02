using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniTaskManager.Core;
using MiniTaskManager.Core.Services;
using MiniTaskManager.Core.Storage;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// file created in the API project's working folder
var tasksFile = "tasks.api.json";

// DI registrations
builder.Services.AddSingleton<ITaskStorage>(sp => new JsonFileTaskStorage(tasksFile));
builder.Services.AddSingleton<TaskService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new { Message = "MiniTaskManager API is running" }));

app.MapGet("/tasks", (TaskService svc) => Results.Ok(svc.GetAllTasks()));

app.MapGet("/tasks/{id:int}", (int id, TaskService svc) =>
{
    var t = svc.GetTaskById(id);
    return t is null ? Results.NotFound() : Results.Ok(t);
});

app.MapGet("/tasks/pending", (TaskService svc) => Results.Ok(svc.GetPendingTasks()));
app.MapGet("/tasks/completed", (TaskService svc) => Results.Ok(svc.GetCompletedTasks()));

app.MapGet("/tasks/priority", (HttpRequest req, TaskService svc) =>
{
    var q = req.Query["priority"].ToString();
    if (string.IsNullOrWhiteSpace(q))
        return Results.BadRequest(new { error = "priority query required (Low|Medium|High)" });

    if (!Enum.TryParse<TaskPriority>(q, true, out var p))
        return Results.BadRequest(new { error = "invalid priority value" });

    return Results.Ok(svc.GetTasksByPriority(p));
});

app.MapPost("/tasks", async (HttpRequest req, TaskService svc) =>
{
    var dto = await req.ReadFromJsonAsync<CreateTaskDto>();
    if (dto is null || string.IsNullOrWhiteSpace(dto.Title))
        return Results.BadRequest(new { error = "Title is required" });

    var priority = dto.Priority ?? TaskPriority.Medium;
    var task = svc.AddTask(dto.Title, priority, dto.DueDate);
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id:int}/complete", (int id, TaskService svc) =>
{
    var ok = svc.MarkCompleted(id);
    return ok ? Results.Ok(new { message = "Marked completed" }) : Results.NotFound();
});

app.MapDelete("/tasks/{id:int}", (int id, TaskService svc) =>
{
    var ok = svc.DeleteTask(id);
    return ok ? Results.Ok(new { message = "Deleted" }) : Results.NotFound();
});

app.Run();

internal class CreateTaskDto
{
    public string? Title { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
