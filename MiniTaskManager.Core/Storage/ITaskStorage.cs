using System.Collections.Generic;
using MiniTaskManager.Core;

namespace MiniTaskManager.Core.Storage
{
    public interface ITaskStorage
    {
        List<TaskItem> LoadTasks();
        void SaveTasks(List<TaskItem> tasks);
    }
}
