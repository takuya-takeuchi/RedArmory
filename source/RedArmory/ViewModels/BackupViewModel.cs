using System;
using System.Collections.ObjectModel;
using System.Linq;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class BackupViewModel : BitnamiStackCommonViewModel<BackupModel>
    {

        #region コンストラクタ

        public BackupViewModel(IApplicationSettingService applicationSettingService, IBitnamiRedmineService bitnamiRedmineService, IBackupService backupService, ILoggerService loggerService)
            : base(loggerService)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<BackupModel>(bitNamiRedmineStacks.Select(
                stack => new BackupModel(applicationSettingService, backupService, loggerService, stack)));
        }

        #endregion

    }

}