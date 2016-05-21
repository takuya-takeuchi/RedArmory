using System;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;
using RedArmory.ViewModels;
using RedArmory.Views.Dialogs;

namespace RedArmory.Models.Services.Dialog
{

    /// <summary>
    /// ダイアログの表示機能を提供します。このクラスは継承できません。
    /// </summary>
    public sealed class ProgressDialogService : IDialogService
    {

        #region イベント
        #endregion

        #region フィールド
        #endregion

        #region コンストラクタ
        #endregion

        #region プロパティ

        public Action Action
        {
            get;
            set;
        }

        public BackupRestoreProgressReport Report
        {
            get;
            set;
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

        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            return null;
        }

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            return null;
        }

        public async Task ShowMessage(string message, string title)
        {
            var viewModel = new ProgressDialogViewModel
            {
                Report = this.Report,
                Title = title
            };

            var view = new ProgressDialog
            {
                DataContext = viewModel
            };

            await DialogHost.Show(view, "RootDialog", new DialogOpenedEventHandler((sender, args) =>
            {
                Task.Factory.StartNew(() =>
                {
                    this.Action();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DialogHost.CloseDialogCommand.Execute(MessageBoxResult.OK, view);
                    });
                });
            }));
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            return null;
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            return null;
        }

        public Task ShowMessageBox(string message, string title)
        {
            return null;
        }

        #endregion

    }

}