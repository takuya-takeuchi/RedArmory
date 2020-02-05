using System;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.Models
{

    internal abstract class BackupRestoreModel : ViewModelBase
    {

        #region Fields

        protected readonly IApplicationSettingService _ApplicationSettingService;

        protected readonly IBitnamiRedmineService _BitnamiRedmineService;

        protected readonly IBackupService _BackupService;

        protected readonly IDispatcherService _DispatcherService;

        protected readonly IDialogService _DialogService;

        protected readonly ILogService _LogService;

        private readonly BitnamiRedmineStack _Stack;

        #endregion

        #region Constructors

        protected BackupRestoreModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService,
            IDialogService dialogService,
            ILogService logService, 
            BitnamiRedmineStack stack)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (dispatcherService == null)
                throw new ArgumentNullException(nameof(dispatcherService));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            this._ApplicationSettingService = applicationSettingService;
            this._BitnamiRedmineService = bitnamiRedmineService;
            this._BackupService = backupService;
            this._DispatcherService = dispatcherService;
            this._DialogService = dialogService;
            this._LogService = logService;
            this._Stack = stack;

            this.Database = true;
            this.Files = true;
            this.Plugins = true;
            this.Themes = true;

            this.Command = new RelayCommand(this.Execute, this.CanExecute);
            this.SelectDirectoryCommand = new RelayCommand(this.ExecuteSelectDirectory, this.CanExecuteSelectDirectory);

            //var bitNamiRedmineStackConfiguration = ConfigurationService.Instance.GetBitnamiRedmineStackConfiguration(stack.DisplayVersion);
            //bitNamiRedmineStackConfiguration.
        }

        #endregion

        #region Properties

        public RelayCommand Command
        {
            get;
            protected set;
        }

        public RelayCommand SelectDirectoryCommand
        {
            get;
            protected set;
        }

        private string _Directory;

        public string Directory
        {
            get
            {
                return this._Directory;
            }
            set
            {
                this._Directory = value;
                this.RaisePropertyChanged();
            }
        }

        public BitnamiRedmineStack Stack
        {
            get
            {
                return this._Stack;
            }
        }

        private bool _Database;

        public bool Database
        {
            get
            {
                return this._Database;
            }
            set
            {
                this._Database = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _Files;

        public bool Files
        {
            get
            {
                return this._Files;
            }
            set
            {
                this._Files = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasDatabase;

        public bool HasDatabase
        {
            get
            {
                return this._HasDatabase;
            }
            set
            {
                this._HasDatabase = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasFile;

        public bool HasFile
        {
            get
            {
                return this._HasFile;
            }
            set
            {
                this._HasFile = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasPlugin;

        public bool HasPlugin
        {
            get
            {
                return this._HasPlugin;
            }
            set
            {
                this._HasPlugin = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasTheme;

        public bool HasTheme
        {
            get
            {
                return this._HasTheme;
            }
            set
            {
                this._HasTheme = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _Plugins;

        public bool Plugins
        {
            get
            {
                return this._Plugins;
            }
            set
            {
                this._Plugins = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _Themes;

        public bool Themes
        {
            get
            {
                return this._Themes;
            }
            set
            {
                this._Themes = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        #endregion

        #region Methods

        protected abstract bool CanExecute();

        protected abstract bool CanExecuteSelectDirectory();

        protected abstract void Execute();

        protected abstract void ExecuteSelectDirectory();

        protected ApplicationSetting GetApplicationSetting(out RedmineSetting redmineSetting)
        {
            var applicationSetting = this._ApplicationSettingService.GetApplicationSetting();
            redmineSetting = applicationSetting.RedmineSettings.
               FirstOrDefault(setting => this.Stack.DisplayVersion.Equals(setting.DisplayVersion));
            if (redmineSetting == null)
            {
                redmineSetting = new RedmineSetting
                {
                    DisplayVersion = this.Stack.DisplayVersion,
                    Backup =
                    {
                        BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                        DirectoryName = "%SHORTDATE% (%VERSION%)",
                        Database = true,
                        Files = true,
                        Plugins = true,
                        Themes = true
                    }
                };
                applicationSetting.RedmineSettings.Add(redmineSetting);
            }

            return applicationSetting;
        }

        protected void UpdateRedmineSetting(ApplicationSetting applicationSetting)
        {
            this._ApplicationSettingService.UpdateApplicationSetting(applicationSetting);
        }

        #region Helpers

        protected void RaiseCanExecuteBackupRestoreChanged()
        {
            if (this.Command != null)
            {
                this.Command.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #endregion

    }

}
