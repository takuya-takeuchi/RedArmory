using System.Collections.ObjectModel;
using System.Linq;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class BackupViewModel : BitnamiStackCommonViewModel<BackupModel>
    {

        #region コンストラクタ

        public BackupViewModel()
        {
            var bitNamiRedmineStacks = BitnamiRedmineService.Instance.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<BackupModel>(bitNamiRedmineStacks.Select(stack => new BackupModel(stack)));
        }

        #endregion

    }

}