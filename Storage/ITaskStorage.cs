using System.Collections.Generic;

namespace MiniTaskManager.App.Storage
{
    public interface ITaskStorage
    {
        List<TaskItem> LoadTasks();
        void SaveTasks(List<TaskItem> tasks);
    }
}
