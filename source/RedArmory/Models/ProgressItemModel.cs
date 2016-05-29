using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace RedArmory.Models
{

    public sealed class ProgressItemModel : ViewModelBase
    {

        #region コンストラクタ

        public ProgressItemModel()
        {
            this.ErrorMessages = new ObservableCollection<string>();
        }

        #endregion

        #region プロパティ

        private ObservableCollection<string> _ErrorMessages;

        public ObservableCollection<string> ErrorMessages
        {
            get
            {
                return this._ErrorMessages;
            }
            set
            {
                this._ErrorMessages = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Key;

        public string Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
                this.RaisePropertyChanged();
            }
        }

        private string _TaskName;

        public string TaskName
        {
            get
            {
                return this._TaskName;
            }
            set
            {
                this._TaskName = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _Progress;
        
        public ProgressState Progress
        {
            get
            {
                return this._Progress;
            }
            set
            {
                this._Progress = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}