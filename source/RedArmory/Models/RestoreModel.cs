using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using RedArmory.Extensions;
using RedArmory.Models.Services;
using RedArmory.Models.Services.Dialog;
using RedArmory.Properties;

namespace RedArmory.Models
{

    public sealed class RestoreModel : BackupRestoreModel
    {

        #region フィールド
        #endregion

        #region コンストラクタ

        public RestoreModel(IApplicationSettingService applicationSettingService, IBitnamiRedmineService bitnamiRedmineService, IBackupService backupService, ILoggerService loggerService, BitnamiRedmineStack stack)
            : base(applicationSettingService, bitnamiRedmineService, backupService, loggerService, stack)
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

            var collectionView = CollectionViewSource.GetDefaultView(this._ApplicationSettingService.BackupHistories);

            var version = this.Stack.DisplayVersion.ToVersion();
            collectionView.Filter = x =>
            {
                var setting = (BackupHistorySetting)x;
                return setting.DisplayVersion.ToVersion() <= version;
            };
            collectionView.SortDescriptions.Add(new SortDescription("DateTime", ListSortDirection.Descending));

            this.BackupHistories = collectionView;

            this.DeleteHistoryCommand = new RelayCommand<BackupHistorySetting>(this.DeleteHistoryExecute);
        }

        #endregion

        #region プロパティ

        private ICollectionView _BackupHistories;

        public ICollectionView BackupHistories
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

        #region メソッド

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

            var yesNoDialogService = new YesNoDialogService();
            await yesNoDialogService.ShowMessage(Resources.Msg_DeleteHistory, null);
            if (yesNoDialogService.Result == MessageBoxResult.No)
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
            var path = this.Directory;

            var configuration = new BackupConfiguration
            {
                Database = this.HasDatabase && this.Database,
                Files = this.HasFile && this.Files,
                Plugins = this.HasPlugin && this.Plugins,
                Themes = this.HasTheme && this.Themes
            };

            // MySql 以外のサービスの停止 (Databaseの更新がないなら、MySql も停止)
            if (!await this.ControlServices(new ServiceConfiguration
            {
                Apache = false,
                Redmine = false,
                MySql = configuration.Database,
                Subversion = false
            }))
            {
                return;
            }

            // バックアップ処理の実行
            string message = null;

            try
            {
                var progressDialogService = new ProgressDialogService();
                var report = new ProgressReportsModel(new[]
                {
                    new ProgressItemModel {Name = Resources.Word_Database, Progress = ProgressState.NotStart},
                    new ProgressItemModel {Name = Resources.Word_Plugin, Progress = ProgressState.NotStart},
                    new ProgressItemModel {Name = Resources.Word_Theme, Progress = ProgressState.NotStart},
                    new ProgressItemModel {Name = Resources.Word_AttachedFile, Progress = ProgressState.NotStart},
                });

                progressDialogService.Action =
                    () =>
                        this._BackupService.Restore(this.Stack, configuration, path, new Progress<ProgressReportsModel>(
                            progressReport =>
                            {
                                foreach (var p in progressReport.Progresses)
                                    report.UpdateProgress(p.Name, p.Progress);
                            }));

                progressDialogService.Report = report;
                await progressDialogService.ShowMessage(null, null);

                message = Resources.Msg_RestoreComplete;
            }
            catch (Exception ex)
            {
                message = $"Exception is thown. Reason is {ex.Message}";
                this._LoggerService.Error(message);

                message = $"StackTrace is {ex.StackTrace}";
                this._LoggerService.Error(message);

                message = Resources.Msg_BackupFailed;
            }
            finally
            {
                var dialogService = new OKDialogService();
                await dialogService.ShowMessage(message, null);
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

        #region ヘルパーメソッド

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