using System;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory.Models.Services.Dialog
{

    /// <summary>
    /// ダイアログの表示機能を提供します。このクラスは抽象クラスです。
    /// </summary>
    internal abstract class DialogServiceBase<T> : GalaSoft.MvvmLight.Views.IDialogService
        where T : FrameworkElement, new()
    {

        #region Events
        #endregion

        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties

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

        #region Methods

        #region Overrides
        #endregion

        #region Event Handlers
        #endregion

        #region Helpers
        #endregion

        #endregion

        #region IDialogService Members

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