using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class SubNavigatorViewModel : ViewModelBase
    {

        #region Properties

        private object _Content;

        public object Content
        {
            get
            {
                return this._Content;
            }
            set
            {
                this._Content = value;
                this.RaisePropertyChanged();
            }
        }

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

        #endregion

    }

}