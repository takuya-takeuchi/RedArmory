using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using RedArmory.Models;

namespace RedArmory.ViewModels
{

    public abstract class BitnamiStackCommonViewModel<T> : ViewModelBase
        where T : BackupRestoreModel
    {

        #region コンストラクタ

        protected BitnamiStackCommonViewModel()
        {
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

        private BackupRestoreModel _SelectedStack;

        public BackupRestoreModel SelectedStack
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