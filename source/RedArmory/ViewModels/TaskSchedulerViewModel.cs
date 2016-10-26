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

        #region フィールド

        private readonly BitnamiRedmineStack _Stack;

        private readonly ITaskService _TaskService;

        private readonly IDatabaseConnectorService _DatabaseConnectorService;

        private readonly EnumerationType _EnumerationType;

        private readonly ProjectItem _Project;

        private bool _SuspendItemOnPropertyChanged;

        #endregion

        #region コンストラクタ

        internal TaskSchedulerViewModel(BitnamiRedmineStack stack, ITaskService taskService)
        {
            if (stack == null)
                throw new ArgumentNullException(nameof(stack));
            if (taskService == null)
                throw new ArgumentNullException(nameof(taskService));

            this._Stack = stack;
            this._TaskService = taskService;

            this.ActiveCommand = new RelayCommand(this.ActiveExecute, this.CanActiveExecute);
            this.DeleteCommand = new RelayCommand(this.DeleteExecute, this.CanDeleteExecute);
            this.RefreshCommand = new RelayCommand(this.RefresExecute, () => true);

            this.Items.CollectionChanged += this.ItemsOnCollectionChanged<TaskSchedulerItem>;
        }

        #endregion

        #region プロパティ

        private readonly ObservableCollection<TaskSchedulerItem> _Items = new ObservableCollection<TaskSchedulerItem>();

        public ObservableCollection<TaskSchedulerItem> Items
        {
            get
            {
                return this._Items;
            }
        }

        public RelayCommand ActiveCommand
        {
            get;
            protected set;
        }

        public RelayCommand DeleteCommand
        {
            get;
            protected set;
        }

        public RelayCommand RefreshCommand
        {
            get;
            protected set;
        }

        #endregion

        #region メソッド

        #region オーバーライド

        #endregion

        #region イベントハンドラ

        private void ItemsOnCollectionChanged<T>(object sender, NotifyCollectionChangedEventArgs e)
            where T : INotifyPropertyChanged
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems)
                    {
                        if (item != null)
                            item.PropertyChanged -= this.ItemOnPropertyChanged;
                    }
                    foreach (T item in e.NewItems)
                    {
                        if (item != null)
                            item.PropertyChanged += this.ItemOnPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        if (item != null)
                            item.PropertyChanged += this.ItemOnPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems)
                    {
                        if (item != null)
                            item.PropertyChanged -= this.ItemOnPropertyChanged;
                    }
                    break;
            }
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var enumerationItem = sender as TaskSchedulerItem;
            if (enumerationItem != null)
            {
                if (this._SuspendItemOnPropertyChanged)
                {
                    return;
                }

                //switch (propertyChangedEventArgs.PropertyName)
                //{
                //    case nameof(enumerationItem.IsDefault):
                //        // 他の要素の変更で ItemOnPropertyChanged は無視したい
                //        this._SuspendItemOnPropertyChanged = true;
                //        foreach (var item in this.Items.Where(item => item != enumerationItem))
                //            item.IsDefault = false;
                //        this._SuspendItemOnPropertyChanged = false;
                //        break;
                //}
            }
        }

        #endregion

        #region ヘルパーメソッド

        private bool CanActiveExecute()
        {
            return true;
        }

        private bool CanDeleteExecute()
        {
            return true;
        }

        private void ActiveExecute()
        {
        }

        private void DeleteExecute()
        {
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

        #endregion

        #endregion

    }
}