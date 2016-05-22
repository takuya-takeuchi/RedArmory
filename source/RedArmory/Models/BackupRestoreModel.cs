﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedArmory.Models.Services;
using RedArmory.Models.Services.Dialog;
using RedArmory.Properties;

namespace RedArmory.Models
{

    public abstract class BackupRestoreModel : ViewModelBase
    {

        #region フィールド

        protected readonly IApplicationSettingService _ApplicationSettingService;

        protected readonly IBitnamiRedmineService _BitnamiRedmineService;

        protected readonly IBackupService _BackupService;

        protected readonly ILoggerService _LoggerService;

        private readonly BitnamiRedmineStack _Stack;

        #endregion

        #region コンストラクタ

        protected BackupRestoreModel(IApplicationSettingService applicationSettingService, IBitnamiRedmineService bitnamiRedmineService, IBackupService backupService, ILoggerService loggerService, BitnamiRedmineStack stack)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            this._ApplicationSettingService = applicationSettingService;
            this._BitnamiRedmineService = bitnamiRedmineService;
            this._BackupService = backupService;
            this._LoggerService = loggerService;
            this._Stack = stack;

            this.Database = true;
            this.Files = true;
            this.Plugins = true;
            this.Themes = true;

            this.Command = new RelayCommand(this.Execute, this.CanExecute);
            this.SelectDirectoryCommand = new RelayCommand(this.ExecuteSelectDirectory, this.CanExecuteSelectDirectory);


#if DEBUG
            var dest = @"f:\RedArmony\Test";
            System.IO.Directory.CreateDirectory(dest);
            this.Directory = dest;
#endif
            //var bitNamiRedmineStackConfiguration = ConfigurationService.Instance.GetBitnamiRedmineStackConfiguration(stack.DisplayVersion);
            //bitNamiRedmineStackConfiguration.
        }

        #endregion

        #region プロパティ

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

        #region メソッド

        protected abstract bool CanExecute();

        protected abstract bool CanExecuteSelectDirectory();

        protected async Task<bool> ControlServices(ServiceConfiguration serviceConfiguration)
        {
            try
            {
                var allServices = this._BitnamiRedmineService.GetServiceDisplayNames(this.Stack, new ServiceConfiguration
                {
                    Apache = true,
                    MySql = true,
                    Redmine = true,
                    Subversion = true
                });

                var report = new ProgressReportsModel(
                    allServices.Select(status => new ProgressItemModel
                    {
                        Name = status.ServiceName,
                        Progress = ProgressState.NotStart
                    }));

                var progressDialogService = new ProgressDialogService
                {
                    Action = () => this._BitnamiRedmineService.ControlService(
                        this.Stack,
                        serviceConfiguration,
                        new Progress<ProgressReportsModel>(
                            progressReport =>
                            {
                                foreach (var p in progressReport.Progresses)
                                    report.UpdateProgress(p.Name, p.Progress);
                            })),
                    Report = report
                };

                await progressDialogService.ShowMessage(null, null);
            }
            catch (Exception ex)
            {
                var message = Resources.Msg_BackupFailed;
                await new OKDialogService().ShowMessage(message, null);

                this._LoggerService.Error(message);

                message = $"Exception is thown. Reason is {ex.Message}";
                this._LoggerService.Error(message);

                message = $"StackTrace is {ex.StackTrace}";
                this._LoggerService.Error(message);

                return false;
            }

            return true;
        }

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

        #region ヘルパーメソッド

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
