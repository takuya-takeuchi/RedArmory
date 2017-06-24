using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class RestoreViewModel : BitnamiStackCommonViewModel<RestoreModel>
    {

        #region コンストラクタ

        public RestoreViewModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService,
            IDialogService dialogService,
            ILogService logService)
            : base(dialogService, logService)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (dispatcherService == null)
                throw new ArgumentNullException(nameof(dispatcherService));

            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<RestoreModel>(bitNamiRedmineStacks.Select(
                stack => new RestoreModel(applicationSettingService, bitnamiRedmineService, backupService, dispatcherService, dialogService, logService, stack)));
        }

        #endregion

    }

}