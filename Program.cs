using MiniTaskManager.App;
using MiniTaskManager.App.Services;
using MiniTaskManager.App.Storage;

using System;
using System.Linq;

var storage = new JsonFileTaskStorage("tasks.json");
TaskService taskService = new TaskService(storage);


while (true)
{
    Console.Clear();
    Console.WriteLine("==== Mini Task Manager ====");
    Console.WriteLine("1. Add Task");
    Console.WriteLine("2. View Tasks");
    Console.WriteLine("3. Mark Task as Completed");
    Console.WriteLine("4. Delete Task");
    Console.WriteLine("5. Exit");
    Console.Write("Enter your choice: ");

    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int choice))
    {
        Console.WriteLine("Invalid input. Press any key to continue...");
        Console.ReadKey();
        continue;
    }

    switch (choice)
    {
        case 1:
            AddTask();
            break;
        case 2:
            ViewTasks();
            break;
        case 3:
            MarkTaskCompleted();
            break;
        case 4:
            DeleteTask();
            break;
        case 5:
            Console.WriteLine("Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid choice. Press any key to continue...");
            Console.ReadKey();
            break;
    }
}

void AddTask()
{
    Console.Clear();
    Console.WriteLine("==== Add New Task ====");
    Console.Write("Enter task title: ");
    string? title = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Title cannot be empty. Press any key to go back...");
        Console.ReadKey();
        return;
    }

    var task = taskService.AddTask(title);
    Console.WriteLine($"Task added successfully! (Id: {task.Id})");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

void ViewTasks()
{
    Console.Clear();
    Console.WriteLine("==== Your Tasks ====");

    var tasks = taskService.GetAllTasks();

    if (!tasks.Any())
    {
        Console.WriteLine("No tasks found.");
    }
    else
    {
        foreach (var task in tasks)
        {
            string status = task.IsCompleted ? "[Done]" : "[Pending]";
            Console.WriteLine($"{task.Id}. {status} {task.Title}");
        }
    }

    Console.WriteLine("\nPress any key to go back to menu...");
    Console.ReadKey();
}

void MarkTaskCompleted()
{
    Console.Clear();
    Console.WriteLine("==== Mark Task as Completed ====");

    if (!taskService.HasTasks())
    {
        Console.WriteLine("No tasks available. Press any key to go back...");
        Console.ReadKey();
        return;
    }

    ViewTasksInline();

    Console.Write("\nEnter Task Id to mark as completed: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int id))
    {
        Console.WriteLine("Invalid Id. Press any key to go back...");
        Console.ReadKey();
        return;
    }

    bool success = taskService.MarkCompleted(id);

    if (!success)
    {
        Console.WriteLine("Task not found or already completed.");
    }
    else
    {
        Console.WriteLine("Task marked as completed!");
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

void DeleteTask()
{
    Console.Clear();
    Console.WriteLine("==== Delete Task ====");

    if (!taskService.HasTasks())
    {
        Console.WriteLine("No tasks available. Press any key to go back...");
        Console.ReadKey();
        return;
    }

    ViewTasksInline();

    Console.Write("\nEnter Task Id to delete: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int id))
    {
        Console.WriteLine("Invalid Id. Press any key to go back...");
        Console.ReadKey();
        return;
    }

    bool success = taskService.DeleteTask(id);

    if (!success)
    {
        Console.WriteLine("Task not found.");
    }
    else
    {
        Console.WriteLine("Task deleted successfully!");
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

void ViewTasksInline()
{
    var tasks = taskService.GetAllTasks();

    foreach (var task in tasks)
    {
        string status = task.IsCompleted ? "[Done]" : "[Pending]";
        Console.WriteLine($"{task.Id}. {status} {task.Title}");
    }
}
