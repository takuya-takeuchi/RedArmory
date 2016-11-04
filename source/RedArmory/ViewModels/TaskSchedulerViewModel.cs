using System;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.Models.Services.Dialog;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.ViewModels
{

    internal class TaskSchedulerViewModel : ViewModelBase
    {

        #region フィールド

        private readonly ITaskService _TaskService;

        #endregion

        #region コンストラクタ

        internal TaskSchedulerViewModel(BitnamiRedmineStack stack, ITaskService taskService)
        {
            if (stack == null)
                throw new ArgumentNullException(nameof(stack));
            if (taskService == null)
                throw new ArgumentNullException(nameof(taskService));
            
            this._TaskService = taskService;

            this.DeleteCommand = new RelayCommand<TaskSchedulerItem>(this.DeleteExecute, this.CanDeleteExecute);
            this.RefreshCommand = new RelayCommand(this.RefresExecute, this.CanRefreshExecute);
            this.RunCommand = new RelayCommand<TaskSchedulerItem>(this.RunExecute, this.CanRunExecute);
            this.StopCommand = new RelayCommand<TaskSchedulerItem>(this.StopExecute, this.CanStopExecute);
        }

        #endregion

        #region プロパティ

        public RelayCommand<TaskSchedulerItem> DeleteCommand
        {
            get;
            protected set;
        }

        private readonly ObservableCollection<TaskSchedulerItem> _Items = new ObservableCollection<TaskSchedulerItem>();

        public ObservableCollection<TaskSchedulerItem> Items
        {
            get
            {
                return this._Items;
            }
        }

        public RelayCommand<TaskSchedulerItem> RunCommand
        {
            get;
            protected set;
        }

        public RelayCommand RefreshCommand
        {
            get;
            protected set;
        }

        public RelayCommand<TaskSchedulerItem> StopCommand
        {
            get;
            protected set;
        }

        #endregion

        #region メソッド

        #region オーバーライド

        #endregion

        #region ヘルパーメソッド

        private bool CanDeleteExecute(TaskSchedulerItem item)
        {
            return true;
        }

        private bool CanRefreshExecute()
        {
            return true;
        }

        private bool CanRunExecute(TaskSchedulerItem item)
        {
            return true;
        }

        private bool CanStopExecute(TaskSchedulerItem item)
        {
            return true;
        }

        private async void DeleteExecute(TaskSchedulerItem item)
        {
            if (item == null)
            {
                return;
            }

            var dialog = new YesNoDialogService();
            await dialog.ShowMessage(Resources.Msg_DeleteTask, null);
            if (dialog.Result == MessageBoxResult.No)
            {
                return;
            }

            if (this._TaskService.Delete(item))
            {
                this.Items.Remove(item);
            }
            else
            {
                // ToDo : ダイアログで警告
            }
        }

        private void RefresExecute()
        {
            var tasks = this._TaskService.GetTasks();

            foreach (var item in this.Items)
                item.Dispose();
            this.Items.Clear();

            foreach (var task in tasks)
                this.Items.Add(new TaskSchedulerItem(task));
        }

        private void RunExecute(TaskSchedulerItem item)
        {
            if (item != null)
            {
                item.Run();
            }
        }

        private void StopExecute(TaskSchedulerItem item)
        {
            if (item != null)
            {
                item.Stop();
            }
        }

        #endregion

        #endregion

    }
}