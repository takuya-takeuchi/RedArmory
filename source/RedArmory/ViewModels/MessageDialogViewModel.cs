using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.ViewModels
{

    public sealed class MessageDialogViewModel : ViewModelBase
    {

        #region イベント
        #endregion

        #region フィールド
        #endregion

        #region コンストラクタ

        public MessageDialogViewModel()
        {
        }

        #endregion

        #region プロパティ

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

        #region メソッド

        #region オーバーライド
        #endregion

        #region イベントハンドラ

        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }

}