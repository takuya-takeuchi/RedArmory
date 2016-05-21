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

        public RestoreModel(BitnamiRedmineStack stack)
            : base(stack)
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
            
            var collectionView = CollectionViewSource.GetDefaultView(ApplicationSettingService.Instance.BackupHistories);

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

            var item = ApplicationSettingService.Instance.BackupHistories.FirstOrDefault(
                setting => setting.OutputDirectory.Equals(historySetting.OutputDirectory));
            if (item != null)
                ApplicationSettingService.Instance.BackupHistories.Remove(item);

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
            string message;

            try
            {
                var path = this.Directory;

                var configuration = new BackupConfiguration
                {
                    Database = this.HasDatabase && this.Database,
                    Files = this.HasFile && this.Files,
                    Plugins = this.HasPlugin && this.Plugins,
                    Themes = this.HasTheme && this.Themes
                };

                var progressDialogService = new ProgressDialogService();
                var report = new BackupRestoreProgressReport();
                progressDialogService.Action = () => BackupService.Instance.Restore(this.Stack, configuration, path, new Progress<BackupRestoreProgressReport>(
                    progressReport =>
                    {
                        report.Database = progressReport.Database;
                        report.Plugin = progressReport.Plugin;
                        report.Theme = progressReport.Theme;
                        report.AttachedFile = progressReport.AttachedFile;
                    }));

                progressDialogService.Report = report;
                await progressDialogService.ShowMessage(null, null);

                message = Resources.Msg_RestoreComplete;
            }
            catch (Exception)
            {
                message = Resources.Msg_RestoreFailed;
            }

            var dialogService = new OKDialogService();
            await dialogService.ShowMessage(message, null);
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
                new BackupConfiguration() : BackupService.Instance.CheckRestoreFolder(this.Stack, this.Directory);

            this.HasDatabase = configuration.Database;
            this.HasTheme = configuration.Themes;
            this.HasPlugin = configuration.Plugins;
            this.HasFile = configuration.Files;
        }

        #endregion

        #endregion

    }

}