using MiniTaskManager.App;
using MiniTaskManager.App.Services;
using MiniTaskManager.App.Storage;

using System;
using System.Linq;

var storage = new JsonFileTaskStorage("tasks.json");
TaskService taskService = new TaskService(storage);


while (true)
{
    Console.WriteLine("==== Mini Task Manager ====");
    Console.WriteLine("1. Add Task");
    Console.WriteLine("2. View All Tasks");
    Console.WriteLine("3. View Pending Tasks");
    Console.WriteLine("4. View Completed Tasks");
    Console.WriteLine("5. View High Priority Tasks");
    Console.WriteLine("6. Mark Task as Completed");
    Console.WriteLine("7. Delete Task");
    Console.WriteLine("8. Exit");
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
        ViewAllTasks();
        break;
    case 3:
        ViewPendingTasks();
        break;
    case 4:
        ViewCompletedTasks();
        break;
    case 5:
        ViewHighPriorityTasks();
        break;
    case 6:
        MarkTaskCompleted();
        break;
    case 7:
        DeleteTask();
        break;
    case 8:
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

    // Priority input
    Console.WriteLine("\nSelect Priority:");
    Console.WriteLine("1. Low");
    Console.WriteLine("2. Medium");
    Console.WriteLine("3. High");
    Console.Write("Enter choice (1-3, default 2): ");
    string? priorityInput = Console.ReadLine();

    TaskPriority priority = TaskPriority.Medium; // default

    if (int.TryParse(priorityInput, out int pChoice))
    {
        if (Enum.IsDefined(typeof(TaskPriority), pChoice))
        {
            priority = (TaskPriority)pChoice;
        }
    }

    // Due date input
    Console.Write("\nEnter due date (yyyy-MM-dd) or press Enter to skip: ");
    string? dueDateInput = Console.ReadLine();

    DateTime? dueDate = null;

    if (!string.IsNullOrWhiteSpace(dueDateInput))
    {
        if (DateTime.TryParse(dueDateInput, out DateTime parsedDate))
        {
            dueDate = parsedDate;
        }
        else
        {
            Console.WriteLine("Invalid date format. Due date will be ignored.");
        }
    }

    var task = taskService.AddTask(title, priority, dueDate);

    Console.WriteLine($"\nTask added successfully! (Id: {task.Id})");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}
void ViewAllTasks()
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
            string dueText = task.DueDate.HasValue
                ? task.DueDate.Value.ToString("yyyy-MM-dd")
                : "No due date";

            Console.WriteLine($"{task.Id}. {status} {task.Title}");
            Console.WriteLine($"    Priority: {task.Priority} | Due: {dueText}");
        }
    }

    Console.WriteLine("\nPress any key to go back to menu...");
    Console.ReadKey();
}
void ViewPendingTasks()
{
    Console.Clear();
    Console.WriteLine("==== Pending Tasks ====");

    var tasks = taskService.GetPendingTasks();

    if (!tasks.Any())
    {
        Console.WriteLine("No pending tasks.");
    }
    else
    {
        foreach (var task in tasks)
        {
            string dueText = task.DueDate.HasValue
                ? task.DueDate.Value.ToString("yyyy-MM-dd")
                : "No due date";

            Console.WriteLine($"{task.Id}. [Pending] {task.Title}");
            Console.WriteLine($"    Priority: {task.Priority} | Due: {dueText}");
        }
    }

    Console.WriteLine("\nPress any key to go back to menu...");
    Console.ReadKey();
}

void ViewCompletedTasks()
{
    Console.Clear();
    Console.WriteLine("==== Completed Tasks ====");

    var tasks = taskService.GetCompletedTasks();

    if (!tasks.Any())
    {
        Console.WriteLine("No completed tasks.");
    }
    else
    {
        foreach (var task in tasks)
        {
            string dueText = task.DueDate.HasValue
                ? task.DueDate.Value.ToString("yyyy-MM-dd")
                : "No due date";

            Console.WriteLine($"{task.Id}. [Done] {task.Title}");
            Console.WriteLine($"    Priority: {task.Priority} | Due: {dueText}");
        }
    }

    Console.WriteLine("\nPress any key to go back to menu...");
    Console.ReadKey();
}

void ViewHighPriorityTasks()
{
    Console.Clear();
    Console.WriteLine("==== High Priority Tasks ====");

    var tasks = taskService.GetTasksByPriority(TaskPriority.High);

    if (!tasks.Any())
    {
        Console.WriteLine("No high priority tasks.");
    }
    else
    {
        foreach (var task in tasks)
        {
            string status = task.IsCompleted ? "[Done]" : "[Pending]";
            string dueText = task.DueDate.HasValue
                ? task.DueDate.Value.ToString("yyyy-MM-dd")
                : "No due date";

            Console.WriteLine($"{task.Id}. {status} {task.Title}");
            Console.WriteLine($"    Priority: {task.Priority} | Due: {dueText}");
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
        string dueText = task.DueDate.HasValue
            ? task.DueDate.Value.ToString("yyyy-MM-dd")
            : "No due date";

        Console.WriteLine($"{task.Id}. {status} {task.Title} | Priority: {task.Priority} | Due: {dueText}");
    }
}

