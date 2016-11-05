using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    internal abstract class BitnamiStackCommonViewModel<T> : ViewModelBase
        where T : BackupRestoreModel
    {

        #region フィールド

        protected readonly IDialogService _DialogService;

        protected readonly ILoggerService _LoggerService;

        #endregion

        #region コンストラクタ

        protected BitnamiStackCommonViewModel(IDialogService dialogService, ILoggerService loggerService)
        {

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            this._DialogService = dialogService;
            this._LoggerService = loggerService;
        }

        #endregion

        #region プロパティ

        private bool _IsEmptyStacks;

        public bool IsEmptyStacks
        {
            get
            {
                return this._IsEmptyStacks;
            }
            private set
            {
                this._IsEmptyStacks = value;
                this.RaisePropertyChanged();
            }
        }

        public bool HasStacks
        {
            get
            {
                return !this._IsEmptyStacks;
            }
            set
            {
                this.RaisePropertyChanged();
            }
        }

        private T _SelectedStack;

        public T SelectedStack
        {
            get
            {
                return this._SelectedStack;
            }
            set
            {
                this._SelectedStack = value;

                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<T> _Stacks;

        public ObservableCollection<T> Stacks
        {
            get
            {
                return this._Stacks;
            }
            protected set
            {
                this._Stacks = value;
                this.RaisePropertyChanged();

                this.IsEmptyStacks = value == null || !this.Stacks.Any();
            }
        }

        #endregion

        #region メソッド

        #region ヘルパーメソッド
        #endregion

        #endregion

    }

}