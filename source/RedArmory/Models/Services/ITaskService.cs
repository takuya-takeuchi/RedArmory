using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    public interface ITaskService
    {

        void Create(TaskSetting taskSetting);

        bool Delete(TaskSchedulerItem item);

        IEnumerable<Microsoft.Win32.TaskScheduler.Task> GetTasks();

    }

}
