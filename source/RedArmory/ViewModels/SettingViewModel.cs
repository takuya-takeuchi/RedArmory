using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class SettingViewModel : ViewModelBase
    {

        #region Fields

        private readonly ILogService _LogService;

        public static ServiceStartupType[] ServiceStartupTypes;

        #endregion

        #region Constructors

        static SettingViewModel()
        {
            ServiceStartupTypes = new[]
            {
                ServiceStartupType.Automatic,
                ServiceStartupType.DelayStart,
                ServiceStartupType.Manual,
                ServiceStartupType.Disabled
            };
        }

        public SettingViewModel(IBitnamiRedmineService bitnamiRedmineService, IRedmineDatabaseConfigurationService databaseConfigurationService, ITaskService taskService, IDialogService dialogService, ILogService logService)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (databaseConfigurationService == null)
                throw new ArgumentNullException(nameof(databaseConfigurationService));

            if (taskService == null)
                throw new ArgumentNullException(nameof(taskService));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            this._LogService = logService;
            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Settings = new ObservableCollection<Setting>(bitNamiRedmineStacks.Select(stack => new Setting(bitnamiRedmineService, databaseConfigurationService, taskService, dialogService, logService, stack)));
            this.IsEmptyStacks = !this.Settings.Any();
        }

        #endregion

        #region Properties

        private bool _IsEmptyStacks;

        public bool IsEmptyStacks
        {
            get
            {
                return this._IsEmptyStacks;
            }
            set
            {
                this._IsEmptyStacks = value;
                this.RaisePropertyChanged();
            }
        }

        public bool HasStacks
        {
            get
            {
                return !this._IsEmptyStacks;
            }
            set
            {
                this.RaisePropertyChanged();
            }
        }

        private Setting _SelectedSetting;

        public Setting SelectedSetting
        {
            get
            {
                return this._SelectedSetting;
            }
            set
            {
                this._SelectedSetting = value;

                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<Setting> _Settings;

        public ObservableCollection<Setting> Settings
        {
            get
            {
                return this._Settings;
            }
            set
            {
                this._Settings = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        #region Event Handlers
        #endregion

        #region Helpers

        #endregion

        #endregion

    }

}