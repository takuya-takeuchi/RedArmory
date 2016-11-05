using System.Threading.Tasks;
using System.Windows;

namespace Ouranos.RedArmory.Models.Services
{

    public interface IDialogService
    {

        Task ShowMessage(MessageBoxButton button, string message, string title);

    }

}
