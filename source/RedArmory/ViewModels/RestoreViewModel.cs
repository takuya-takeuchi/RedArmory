using System;
using System.Collections.ObjectModel;
using System.Linq;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class RestoreViewModel : BitnamiStackCommonViewModel<RestoreModel>
    {

        #region コンストラクタ

        public RestoreViewModel(IApplicationSettingService applicationSettingService, IBitnamiRedmineService bitnamiRedmineService, IBackupService backupService, ILoggerService loggerService)
            : base(loggerService)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<RestoreModel>(bitNamiRedmineStacks.Select(
                stack => new RestoreModel(applicationSettingService, bitnamiRedmineService, backupService, loggerService, stack)));
        }

        #endregion

    }

}