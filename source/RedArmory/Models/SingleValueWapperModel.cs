using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.Models
{

    internal sealed class SingleValueWapperModel<T> : ViewModelBase
    {

        #region Properties

        private T _Value;

        public T Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}
