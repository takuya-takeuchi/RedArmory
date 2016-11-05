using System;
using System.Threading.Tasks;
using System.Windows;
using Ouranos.RedArmory.Models.Services.Dialog;

namespace Ouranos.RedArmory.Models.Services
{

    public sealed class DialogService : IDialogService
    {

        #region IDialogService メンバー

        public async Task ShowMessage(MessageBoxButton button, string message, string title)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    await new OKDialogService().ShowMessage(message, title);
                    break;
                case MessageBoxButton.OKCancel:
                    break;
                case MessageBoxButton.YesNoCancel:
                    break;
                case MessageBoxButton.YesNo:
                    await new YesNoDialogService().ShowMessage(message, title);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        #endregion

    }

}