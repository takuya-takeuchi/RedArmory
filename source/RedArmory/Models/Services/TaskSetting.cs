using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.Models.Services
{

    public sealed class TaskSetting
    {

        #region プロパティ

        public string Argument
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Trigger Trigger
        {
            get;
            set;
        }

        #endregion

    }

}