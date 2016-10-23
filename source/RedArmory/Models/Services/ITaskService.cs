using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    public interface ITaskService
    {

        void Create(TaskSetting taskSetting);

        IEnumerable<Microsoft.Win32.TaskScheduler.Task> GetTasks();

    }

}
