using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    public abstract class BitnamiStackCommonViewModel<T> : ViewModelBase
        where T : BackupRestoreModel
    {

        #region �t�B�[���h

        protected readonly ILoggerService _LoggerService;

        #endregion

        #region �R���X�g���N�^

        protected BitnamiStackCommonViewModel(ILoggerService loggerService)
        {
            this._LoggerService = loggerService;
        }

        #endregion

        #region �v���p�e�B

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

        #region ���\�b�h

        #region �w���p�[���\�b�h
        #endregion

        #endregion

    }

}