using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MiniTaskManager.Core.Storage
{
    public class JsonFileTaskStorage : ITaskStorage
    {
        private readonly string _filePath;

        public JsonFileTaskStorage(string filePath)
        {
            _filePath = filePath;
        }

        public List<TaskItem> LoadTasks()
        {
            if (!File.Exists(_filePath))
                return new List<TaskItem>();

            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<TaskItem>();

            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, json);
        }
    }
}
