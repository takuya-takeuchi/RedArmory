using System;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Ouranos.RedArmory.ViewModels;
using Ouranos.RedArmory.Views.Dialogs;

namespace Ouranos.RedArmory.Models.Services.Dialog
{

    /// <summary>
    /// Provides the function to show dialog. This class is sealed.
    /// </summary>
    internal sealed class ProgressDialogService : DialogServiceBase<ProgressDialog>
    {

        #region Events
        #endregion

        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties

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

        #region Methods

        #region Overrides
        #endregion

        #region Event Handlers
        #endregion

        #region Helpers
        #endregion

        #endregion

        #region IDialogService Members

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