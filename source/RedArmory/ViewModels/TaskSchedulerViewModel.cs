using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    internal class TaskSchedulerViewModel : ViewModelBase
    {

        #region �t�B�[���h

        private readonly BitnamiRedmineStack _Stack;

        private readonly ITaskService _TaskService;

        #endregion

        #region �R���X�g���N�^

        internal TaskSchedulerViewModel(BitnamiRedmineStack stack, ITaskService taskService)
        {
            if (stack == null)
                throw new ArgumentNullException(nameof(stack));
            if (taskService == null)
                throw new ArgumentNullException(nameof(taskService));

            this._Stack = stack;
            this._TaskService = taskService;

            this.DeleteCommand = new RelayCommand<TaskSchedulerItem>(this.DeleteExecute, this.CanDeleteExecute);
            this.RefreshCommand = new RelayCommand(this.RefresExecute, this.CanRefreshExecute);
            this.RunCommand = new RelayCommand<TaskSchedulerItem>(this.RunExecute, this.CanRunExecute);
            this.StopCommand = new RelayCommand<TaskSchedulerItem>(this.StopExecute, this.CanStopExecute);
        }

        #endregion

        #region �v���p�e�B

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

        #region ���\�b�h

        #region �I�[�o�[���C�h

        #endregion

        #region �w���p�[���\�b�h

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

        private void DeleteExecute(TaskSchedulerItem item)
        {
            if (item != null)
            {
                if (this._TaskService.Delete(item))
                {
                    this.Items.Remove(item);
                }
                else
                {
                    // ToDo : �_�C�A���O�Ōx��
                }
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