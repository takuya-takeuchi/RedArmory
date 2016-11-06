using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    public interface ITaskService
    {

        void Create(TaskSetting taskSetting);

        bool Delete(TaskSchedulerItem item);

        bool IsExist(string name);

        IEnumerable<Microsoft.Win32.TaskScheduler.Task> GetTasks();

    }

}
