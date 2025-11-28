using System;

namespace MiniTaskManager.App
{
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public class TaskItem
    {
        public int Id { get; set; }                    // Unique ID
        public string Title { get; set; } = "";        // Task title
        public bool IsCompleted { get; set; }          // Status (Done / Not Done)
        public TaskPriority Priority { get; set; }     // Priority
        public DateTime? DueDate { get; set; }         // Optional due date
    }
}
