using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MiniTaskManager.App.Storage
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
            {
                return new List<TaskItem>();
            }

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<TaskItem>();
            }

            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
            return tasks ?? new List<TaskItem>();
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, json);
        }
    }
}
