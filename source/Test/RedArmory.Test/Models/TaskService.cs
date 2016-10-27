using System;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.TaskScheduler;
using Ouranos.RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class TaskService
    {

        [TestMethod]
        public void GetTasks()
        {
            var taskService = SimpleIoc.Default.GetInstance<ITaskService>();

            var tasks = taskService.GetTasks().ToArray();
            Assert.IsTrue(tasks.Length != 0);
            
            foreach (var task in tasks)
            {
                var actions = task.Definition.Actions.
                    Where(action => action is ExecAction).
                    Cast<ExecAction>().
                    Where(action => action.Path.Contains("RedArmory.exe"));

                Console.WriteLine($"Name:{task.Name}, State:{task.State}, Enabled:{task.Enabled}, LastRunTime:{task.LastRunTime}, NextRunTime:{task.NextRunTime}");
                foreach (var action in actions)
                {
                    Console.WriteLine($"\tPath:{action.Path}");
                }
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            SimpleIoc.Default.Register<ILoggerService, LoggerService>();
            SimpleIoc.Default.Register<IDatabaseService, MySqlService>();
            SimpleIoc.Default.Register<IBackupService, BackupService>();
            SimpleIoc.Default.Register<ITaskService, Ouranos.RedArmory.Models.Services.TaskService>();
        }

    }

}
