using System.Collections.ObjectModel;
using System.Linq;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class RestoreViewModel : BitnamiStackCommonViewModel<RestoreModel>
    {

        #region コンストラクタ

        public RestoreViewModel()
        {
            var bitNamiRedmineStacks = BitnamiRedmineService.Instance.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<RestoreModel>(bitNamiRedmineStacks.Select(stack => new RestoreModel(stack)));
        }

        #endregion

    }

}