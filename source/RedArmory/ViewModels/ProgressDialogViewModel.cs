using GalaSoft.MvvmLight;
using RedArmory.Models;

namespace RedArmory.ViewModels
{

    public sealed class ProgressDialogViewModel : ViewModelBase
    {

        #region コンストラクタ
        #endregion

        #region プロパティ

        private ProgressReportsModel _Report;

        public ProgressReportsModel Report
        {
            get
            {
                return this._Report;
            }
            set
            {
                this._Report = value;
                this.RaisePropertyChanged();
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
                this._Title = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        public void Close()
        {

        }

        #endregion

    }

}
