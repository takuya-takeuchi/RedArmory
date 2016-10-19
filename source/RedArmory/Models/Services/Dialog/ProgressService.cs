using System;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Ouranos.RedArmory.ViewModels;
using Ouranos.RedArmory.Views.Dialogs;

namespace Ouranos.RedArmory.Models.Services.Dialog
{

    /// <summary>
    /// ダイアログの表示機能を提供します。このクラスは継承できません。
    /// </summary>
    internal sealed class ProgressDialogService : DialogServiceBase<ProgressDialog>
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

        public bool IsAutoClose
        {
            get;
            set;
        }

        public ProgressReportsModel Report
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

        public override async Task ShowMessage(string message, string title)
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



            MessageBoxResult result;

            result = (MessageBoxResult)await DialogHost.Show(view, "RootDialog", new DialogOpenedEventHandler((sender, args) =>
            {
                Task.Factory.StartNew(() =>
                {
                    this.Action();

                    if (this.IsAutoClose)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            DialogHost.CloseDialogCommand.Execute(MessageBoxResult.OK, view);
                        });
                    }
                });
            }));

            this.Result = result;
        }

        #endregion

    }

}