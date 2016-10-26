using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.Models.Services
{

    internal sealed class TaskService : ITaskService
    {

        #region メソッド

        #region ヘルパーメソッド

        private static IEnumerable<Task> EnumTasks(TaskFolder fld)
        {
            foreach (var task in fld.Tasks)
                yield return task;
            foreach (var sfld in fld.SubFolders)
                foreach (var enumTask in EnumTasks(sfld))
                    yield return enumTask;
        }


        #endregion

        #endregion

        #region TaskService メンバー

        public void Create(TaskSetting taskSetting)
        {
            if (taskSetting == null)
                throw new ArgumentNullException(nameof(taskSetting));

            using (var ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                // Create a new task definition and assign properties
                var td = ts.NewTask();
                td.RegistrationInfo.Description = taskSetting.Description;
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                td.Triggers.Add(taskSetting.Trigger);

                var exePath = Environment.GetCommandLineArgs()[0];
                var exeFullPath = System.IO.Path.GetFullPath(exePath);

                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(exeFullPath, taskSetting.Argument));

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(taskSetting.Name, td);
            }
        }

        public IEnumerable<Task> GetTasks()
        {
            var tasks = new List<Task>();
            using (var ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                var list = (from task in EnumTasks(ts.RootFolder)
                            let actions = task.Definition.Actions.
                            Where(action => action is ExecAction).
                            Cast<ExecAction>().
                            Where(action => action.Path.Contains("RedArmory.exe"))
                            where actions.Any() select task).ToList();
                tasks.AddRange(list);
            }

            return tasks;
        }

        #endregion

    }

}