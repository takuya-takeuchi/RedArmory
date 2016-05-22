using GalaSoft.MvvmLight;

namespace RedArmory.Models
{

    public sealed class ProgressItemModel : ViewModelBase
    {

        #region プロパティ

        private string _Name;

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
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