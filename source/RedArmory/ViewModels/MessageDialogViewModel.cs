using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class MessageDialogViewModel : ViewModelBase
    {

        #region Events
        #endregion

        #region Fields
        #endregion

        #region Constructors

        public MessageDialogViewModel()
        {
        }

        #endregion

        #region Properties

        private string _Message;

        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                if (this._Message != value)
                {
                    this._Message = value;

                    this.RaisePropertyChanged();
                }
            }
        }

        private string _Title;

        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if (this._Title != value)
                {
                    this._Title = value;

                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        #region Overrides
        #endregion

        #region Event Handlers

        #endregion

        #region Helpers
        #endregion

        #endregion

    }

}