namespace MiniTaskManager.App
{
    public class TaskItem
    {
        public int Id { get; set; }              // Unique ID
        public string Title { get; set; } = "";  // Task title
        public bool IsCompleted { get; set; }    // Status (Done / Not Done)
    }
}
