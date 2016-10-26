using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    internal class EnumerationViewModel : ViewModelBase
    {

        #region フィールド

        private readonly IDatabaseConnectorService _DatabaseConnectorService;

        private readonly EnumerationType _EnumerationType;

        private readonly ProjectItem _Project;

        private bool _SuspendItemOnPropertyChanged;

        #endregion

        #region コンストラクタ

        internal EnumerationViewModel(DatabaseConfiguration databaseConfiguration, ProjectItem project, EnumerationType enumerationType)
        {
            if (databaseConfiguration == null)
                throw new ArgumentNullException(nameof(databaseConfiguration));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            this._Project = project;
            this._EnumerationType = enumerationType;

            this._DatabaseConnectorService = new MySqlConnectorService(databaseConfiguration);

            this.RefreshCommand = new RelayCommand(this.RefresExecute, this.CanRefreshExecute);
            this.UpdateCommand = new RelayCommand(this.UpdateExecute, this.CanUpdateExecute);

            this.Items.CollectionChanged += this.ItemsOnCollectionChanged<EnumerationItem>;
        }

        #endregion

        #region プロパティ

        private readonly ObservableCollection<EnumerationItem> _Items = new ObservableCollection<EnumerationItem>();

        public ObservableCollection<EnumerationItem> Items
        {
            get
            {
                return this._Items;
            }
        }

        private RelayCommand<object> _DownCommand;

        public RelayCommand<object> DownCommand
        {
            get
            {
                return this._DownCommand ?? (this._DownCommand = new RelayCommand<object>(o =>
                {
                    var index = this.GetIndex(o as EnumerationItem);
                    if (index == -1)
                    {
                        return;
                    }

                    this.Items.Move(index, index + 1);
                    this.UpdateOrderState();
                }));
            }
        }

        public RelayCommand RefreshCommand
        {
            get;
            protected set;
        }

        private RelayCommand<object> _UpCommand;

        public RelayCommand<object> UpCommand
        {
            get
            {
                return this._UpCommand ?? (this._UpCommand = new RelayCommand<object>(o =>
                {
                    var index = this.GetIndex(o as EnumerationItem);
                    if (index == -1)
                    {
                        return;
                    }

                    this.Items.Move(index, index - 1);
                    this.UpdateOrderState();
                }));
            }
        }

        public RelayCommand UpdateCommand
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
            var enumerationItem = sender as EnumerationItem;
            if (enumerationItem != null)
            {
                if (this._SuspendItemOnPropertyChanged)
                {
                    return;
                }

                switch (propertyChangedEventArgs.PropertyName)
                {
                    case nameof(enumerationItem.IsDefault):
                        // 他の要素の変更で ItemOnPropertyChanged は無視したい
                        this._SuspendItemOnPropertyChanged = true;
                        foreach (var item in this.Items.Where(item => item != enumerationItem))
                            item.IsDefault = false;
                        this._SuspendItemOnPropertyChanged = false;
                        break;
                }
            }
        }

        #endregion

        #region ヘルパーメソッド

        private bool CanRefreshExecute()
        {
            return true;
        }

        private bool CanUpdateExecute()
        {
            return true;
        }

        private int GetIndex(EnumerationItem parameter)
        {
            var filterConfig = parameter;
            if (filterConfig == null)
            {
                return -1;
            }

            var index = this.Items.IndexOf(filterConfig);
            return index;
        }

        private void RefresExecute()
        {
            var projectId = this._Project.Id == 0 ? (int?)null : this._Project.Id;
            var type = this._EnumerationType.ToString();
            var items = this._DatabaseConnectorService.GetEnumerations().
                Where(item => item.ProjectId == projectId).
                Where(item => item.Type == type).
                OrderBy(item => item.Position);

            foreach (var item in items)
                this.Items.Add(item);

            this.UpdateOrderState();
        }

        private void UpdateExecute()
        {
            // Position を整理 (1から始まるはずだが一応)
            var minPosition = this.Items.Min(item => item.Position);
            foreach (var item in this.Items)
                item.Position = (minPosition++);

            this._DatabaseConnectorService.UpdateEnumerations(this.Items);
        }

        private void UpdateOrderState()
        {
            var items = this.Items;
            if (!items.Any())
            {
                return;
            }

            for (int index = 0, count = items.Count; index < count; index++)
            {
                var item = items[index];
                item.IsTop = index == 0;
                item.IsLast = index == count - 1;
            }
        }

        #endregion

        #endregion

    }
}