using System;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;
using RedArmory.ViewModels;

namespace RedArmory.Models.Services.Dialog
{

    /// <summary>
    /// ダイアログの表示機能を提供します。このクラスは抽象クラスです。
    /// </summary>
    public abstract class DialogServiceBase<T> : IDialogService
        where T : FrameworkElement, new()
    {

        #region イベント
        #endregion

        #region フィールド
        #endregion

        #region コンストラクタ
        #endregion

        #region プロパティ

        public DialogClosingEventHandler DialogClosing
        {
            get;
            set;
        }

        public MessageBoxResult Result
        {
            get;
            protected set;
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

        #region IDialogService メンバ

        public virtual Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            return null;
        }

        public virtual Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            return null;
        }

        public virtual async Task ShowMessage(string message, string title)
        {
            var viewModel = new MessageDialogViewModel
            {
                Message = message,
                Title = title
            };

            var view = new T
            {
                DataContext = viewModel
            };

            var result = await DialogHost.Show(view, "RootDialog", (sender, args) =>
            {
                // ProressDialog 表示後、フォーカスが外れているのを防ぐため
                view.Focus();
            }, this.DialogClosing);

            this.Result = (MessageBoxResult) result;
        }

        public virtual Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            return null;
        }

        public virtual Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            return null;
        }

        public virtual Task ShowMessageBox(string message, string title)
        {
            return null;
        }

        #endregion

    }

}