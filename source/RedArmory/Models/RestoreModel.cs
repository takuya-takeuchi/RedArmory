using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Extensions;
using Ouranos.RedArmory.Interfaces;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.Models.Services.Dialog;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Models
{

    internal sealed class RestoreModel : BackupRestoreModel
    {

        #region Fields
        #endregion

        #region Constructors

        public RestoreModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService,
            IDialogService dialogService,
            ILogService logService, 
            BitnamiRedmineStack stack)
            : base(applicationSettingService, bitnamiRedmineService, backupService, dispatcherService, dialogService, logService, stack)
        {
            this.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "Directory":
                        this.UpdateState();
                        break;
                }
            };

            RedmineSetting redmineSetting;
            this.GetApplicationSetting(out redmineSetting);

            this.BackupHistories = new CollectionViewSource
            {
                Source = this._ApplicationSettingService.BackupHistories
            };

            this._ApplicationSettingService.BackupHistories.CollectionChanged +=
                (sender, args) => this.BackupHistories.View.Refresh();

            var version = this.Stack.DisplayVersion.ToVersion();
            this.BackupHistories.View.Filter = x =>
            {
                var setting = (BackupHistorySetting)x;
                var compared = setting.DisplayVersion.ToVersion() <= version;
                return compared;
            };
            this.BackupHistories.View.SortDescriptions.Add(new SortDescription("DateTime", ListSortDirection.Descending));

            this.DeleteHistoryCommand = new RelayCommand<BackupHistorySetting>(this.DeleteHistoryExecute);
        }

        #endregion

        #region Properties

        private CollectionViewSource _BackupHistories;

        public CollectionViewSource BackupHistories
        {
            get
            {
                return this._BackupHistories;
            }
            private set
            {
                this._BackupHistories = value;
                this.RaisePropertyChanged();
            }
        }

        public RelayCommand<BackupHistorySetting> DeleteHistoryCommand
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        protected override bool CanExecute()
        {
            var canAction = this.HasDatabase && this.Database;
            canAction |= (this.HasTheme && this.Themes);
            canAction |= (this.HasPlugin && this.Plugins);
            canAction |= (this.HasFile && this.Files);

            return canAction && this.Stack != null &&
                   !string.IsNullOrWhiteSpace(this.Directory) &&
                   System.IO.Directory.Exists(this.Directory);
        }

        protected override bool CanExecuteSelectDirectory()
        {
            return true;
        }

        private async void DeleteHistoryExecute(BackupHistorySetting historySetting)
        {
            if (historySetting == null)
                return;
            
            var result = await this._DialogService.ShowMessage(MessageBoxButton.YesNo, Resources.Msg_DeleteHistory, null);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            var item = this._ApplicationSettingService.BackupHistories.FirstOrDefault(
                setting => setting.OutputDirectory.Equals(historySetting.OutputDirectory));
            if (item != null)
                this._ApplicationSettingService.BackupHistories.Remove(item);

            RedmineSetting redmineSetting;
            var applicationSetting = this.GetApplicationSetting(out redmineSetting);

            item = applicationSetting.BackupHistories.FirstOrDefault(
                setting => setting.OutputDirectory.Equals(historySetting.OutputDirectory));
            if (item != null)
                applicationSetting.BackupHistories.Remove(item);

            this.UpdateRedmineSetting(applicationSetting);
        }

        protected override async void Execute()
        {
            var configuration = new BackupConfiguration
            {
                Database = this.HasDatabase && this.Database,
                Files = this.HasFile && this.Files,
                Plugins = this.HasPlugin && this.Plugins,
                Themes = this.HasTheme && this.Themes
            };

            // リストア処理の実行
            string message = null;

            try
            {
                var progressDialogService = new ProgressDialogService { IsAutoClose = true };
                
                var engine = new RestoreEngine(this._BitnamiRedmineService,
                                               this._BackupService,
                                               this._DispatcherService,
                                               this._LogService,
                                               configuration,
                                               this.Stack,
                                               this.Directory);

                var report = engine.PrepareRestore();
                progressDialogService.Action = () =>
                {
                    engine.ExecuteRestore();
                };

                progressDialogService.Report = report;
                await progressDialogService.ShowMessage(null, null);
                if (progressDialogService.Result == MessageBoxResult.Cancel)
                {
                    message = Resources.Msg_RestoreCancel;
                }
                else
                {
                    message = Resources.Msg_RestoreComplete;
                }
            }
            catch (Exception ex)
            {
                message = $"Exception is thown. Reason is {ex.Message}";
                this._LogService.Error(message);

                message = $"StackTrace is {ex.StackTrace}";
                this._LogService.Error(message);

                message = Resources.Msg_BackupFailed;
            }
            finally
            {
                await this._DialogService.ShowMessage(MessageBoxButton.OK, message, null);
            }
        }

        protected override void ExecuteSelectDirectory()
        {
            using (var dlg = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog())
            {
                dlg.Title = Resources.Word_SelectSourceDirectoryTitle;
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = this.Directory;

                dlg.AddToMostRecentlyUsedList = false;
                dlg.AllowNonFileSystemItems = false;
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                {
                    this.Directory = dlg.FileName;
                }
            }
        }

        #region Helpers

        private void UpdateState()
        {
            var configuration = string.IsNullOrWhiteSpace(this.Directory) ?
                new BackupConfiguration() : this._BackupService.CheckRestoreFolder(this.Stack, this.Directory);

            this.HasDatabase = configuration.Database;
            this.HasTheme = configuration.Themes;
            this.HasPlugin = configuration.Plugins;
            this.HasFile = configuration.Files;
        }

        #endregion

        #endregion

    }

}