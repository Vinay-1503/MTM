using MiniTaskManager.App;
using System;
using System.Collections.Generic;

// In-memory storage for tasks
List<TaskItem> tasks = new List<TaskItem>();
int nextId = 1;

while (true)
{
    Console.Clear();
    Console.WriteLine("==== Mini Task Manager ====");
    Console.WriteLine("1. Add Task");
    Console.WriteLine("2. View Tasks");
    Console.WriteLine("3. Exit");
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

    var task = new TaskItem
    {
        Id = nextId++,
        Title = title,
        IsCompleted = false
    };

    tasks.Add(task);

    Console.WriteLine("Task added successfully! Press any key to continue...");
    Console.ReadKey();
}

void ViewTasks()
{
    Console.Clear();
    Console.WriteLine("==== Your Tasks ====");

    if (tasks.Count == 0)
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
