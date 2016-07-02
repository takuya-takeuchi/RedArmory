using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.Models
{

    public sealed class AcknowledgmentModel : ViewModelBase
    {

        #region プロパティ

        private string _Author;

        public string Author
        {
            get
            {
                return this._Author;
            }
            set
            {
                this._Author = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Description;

        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Licence;

        public string Licence
        {
            get
            {
                return this._Licence;
            }
            set
            {
                this._Licence = value;
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

        private string _Url;

        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this._Url = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }

}
