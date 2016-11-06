using System;
using System.Threading.Tasks;
using System.Windows;
using Ouranos.RedArmory.Models.Services.Dialog;

namespace Ouranos.RedArmory.Models.Services
{

    public sealed class DialogService : IDialogService
    {

        #region IDialogService メンバー

        public async Task<MessageBoxResult> ShowMessage(MessageBoxButton button, string message, string title)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    var okDialogService = new OKDialogService();
                    await okDialogService.ShowMessage(message, title);
                    return okDialogService.Result;
                case MessageBoxButton.YesNo:
                    var yesNoDialogService = new YesNoDialogService();
                    await yesNoDialogService.ShowMessage(message, title);
                    return yesNoDialogService.Result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        #endregion

    }

}