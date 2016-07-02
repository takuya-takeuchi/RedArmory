using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    public sealed class RestoreViewModel : BitnamiStackCommonViewModel<RestoreModel>
    {

        #region コンストラクタ

        public RestoreViewModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService,
            ILoggerService loggerService)
            : base(loggerService)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (dispatcherService == null)
                throw new ArgumentNullException(nameof(dispatcherService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<RestoreModel>(bitNamiRedmineStacks.Select(
                stack => new RestoreModel(applicationSettingService, bitnamiRedmineService, backupService, dispatcherService, loggerService, stack)));
        }

        #endregion

    }

}