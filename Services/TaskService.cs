using System;
using System.Collections.Generic;
using System.Linq;
using MiniTaskManager.App.Storage;

namespace MiniTaskManager.App.Services
{
    public class TaskService
    {
        private readonly List<TaskItem> _tasks;
        private int _nextId;
        private readonly ITaskStorage _storage;

        public TaskService(ITaskStorage storage)
        {
            _storage = storage;

            // Load existing tasks OR start fresh
            _tasks = _storage.LoadTasks() ?? new List<TaskItem>();

            // Maintain incremental ID
            _nextId = _tasks.Any() ? _tasks.Max(t => t.Id) + 1 : 1;
        }

        // ========================= CRUD ========================= //

        public IReadOnlyList<TaskItem> GetAllTasks()
        {
            return _tasks;
        }

        public TaskItem? GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public TaskItem AddTask(string title, TaskPriority priority, DateTime? dueDate)
        {
            var task = new TaskItem
            {
                Id = _nextId++,
                Title = title,
                IsCompleted = false,
                Priority = priority,
                DueDate = dueDate
            };

            _tasks.Add(task);
            Save();
            return task;
        }

        public bool MarkCompleted(int id)
        {
            var task = GetTaskById(id);
            if (task == null || task.IsCompleted)
                return false;

            task.IsCompleted = true;
            Save();
            return true;
        }

        public bool DeleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task == null)
                return false;

            _tasks.Remove(task);
            Save();
            return true;
        }

        public bool HasTasks()
        {
            return _tasks.Count > 0;
        }


        // ========================= FILTERING ========================= //

        public IReadOnlyList<TaskItem> GetPendingTasks()
        {
            return _tasks.Where(t => !t.IsCompleted).ToList();
        }

        public IReadOnlyList<TaskItem> GetCompletedTasks()
        {
            return _tasks.Where(t => t.IsCompleted).ToList();
        }

        public IReadOnlyList<TaskItem> GetTasksByPriority(TaskPriority priority)
        {
            return _tasks.Where(t => t.Priority == priority).ToList();
        }


        // ========================= SAVE to JSON ========================= //

        private void Save()
        {
            _storage.SaveTasks(_tasks);
        }
    }
}
