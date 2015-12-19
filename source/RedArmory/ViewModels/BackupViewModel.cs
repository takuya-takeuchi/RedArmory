using System.Collections.ObjectModel;
using System.Linq;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class BackupViewModel : BitNamiStackCommonViewModel<BackupModel>
    {

        #region コンストラクタ

        public BackupViewModel()
        {
            var bitNamiRedmineStacks = BitNamiRedmineService.Instance.GetBitNamiRedmineStacks();

            this.Stacks = new ObservableCollection<BackupModel>(bitNamiRedmineStacks.Select(stack => new BackupModel(stack)));
        }

        #endregion

    }

}