using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Extensions;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.Models.Services.Dialog;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Models
{

    public sealed class RestoreModel : BackupRestoreModel
    {

        #region フィールド
        #endregion

        #region コンストラクタ

        public RestoreModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService, 
            ILoggerService loggerService, 
            BitnamiRedmineStack stack)
            : base(applicationSettingService, bitnamiRedmineService, backupService, dispatcherService, loggerService, stack)
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

        #region プロパティ

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

            // リストア処理の実行
            string message = null;

            try
            {
                var progressDialogService = new ProgressDialogService { IsAutoClose = true };

                // Apache
                var apacheServices = this._BitnamiRedmineService.GetServiceDisplayNames(this.Stack, new ServiceConfiguration
                {
                    Apache = true,
                    MySql = false,
                    Redmine = false,
                    Subversion = false
                }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(false)));

                // MySql
                var mySqlServices = this._BitnamiRedmineService.GetServiceDisplayNames(this.Stack, new ServiceConfiguration
                {
                    Apache = false,
                    MySql = true,
                    Redmine = false,
                    Subversion = false
                }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(configuration.Database)));

                // Subversion
                var subversionServices = this._BitnamiRedmineService.GetServiceDisplayNames(this.Stack, new ServiceConfiguration
                {
                    Apache = false,
                    MySql = false,
                    Redmine = false,
                    Subversion = true
                }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(false)));

                // Redmine
                var redmineServices = this._BitnamiRedmineService.GetServiceDisplayNames(this.Stack, new ServiceConfiguration
                {
                    Apache = false,
                    MySql = false,
                    Redmine = true,
                    Subversion = false
                }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(false)));

                var beforeServiceReports =
                    apacheServices.Concat(
                    mySqlServices).Concat(
                    subversionServices).Concat(
                    redmineServices).ToArray();

                var mainTaskReports = new[]
                {
                    new ProgressItemModel {Key = Resources.Word_Database, TaskName = Resources.Word_Database,Progress = ProgressState.NotStart},
                    new ProgressItemModel {Key = Resources.Word_Plugin, TaskName = Resources.Word_Plugin,Progress = ProgressState.NotStart},
                    new ProgressItemModel {Key = Resources.Word_Theme, TaskName = Resources.Word_Theme,Progress = ProgressState.NotStart},
                    new ProgressItemModel {Key = Resources.Word_AttachedFile, TaskName = Resources.Word_AttachedFile,Progress = ProgressState.NotStart},
                };

                var afterServiceReports = this._BitnamiRedmineService.GetServiceDisplayNames(this.Stack, new ServiceConfiguration
                {
                    Apache = true,
                    MySql = true,
                    Redmine = true,
                    Subversion = true
                }).Select(status => status.ToProgressItemModel(true)).ToArray();

                var report = new ProgressReportsModel(this._DispatcherService, beforeServiceReports.Select(pair => pair.Value).Concat(mainTaskReports).Concat(afterServiceReports));

                progressDialogService.Action = () =>
                {
                    // サービスの停止
                    var beforeTargets = new[]
                    {
                        new { Reports = apacheServices,       RequiredStart = false },
                        new { Reports = mySqlServices,        RequiredStart = configuration.Database },
                        new { Reports = subversionServices,   RequiredStart = false },
                        new { Reports = redmineServices,      RequiredStart = false },
                    };

                    var beforeServiceProgress = new Progress<ProgressReportsModel>(progressReport =>
                    {
                        foreach (var p in progressReport.Progresses)
                        {
                            var progressItemModel = beforeServiceReports.Select(kvp => kvp.Value).FirstOrDefault(model => model.Key.Equals(p.Key));
                            if (progressItemModel == null)
                                continue;

                            progressItemModel.Progress = p.Progress;
                            progressItemModel.ErrorMessages = p.ErrorMessages;
                        }
                    });

                    foreach (var t in beforeTargets)
                        foreach (var s in t.Reports)
                            if (t.RequiredStart)
                                this._BitnamiRedmineService.StartService(s.Key, new ProgressReportsModel(this._DispatcherService, new[] { s.Value }), beforeServiceProgress);
                            else
                                this._BitnamiRedmineService.StopService(s.Key, new ProgressReportsModel(this._DispatcherService, new[] { s.Value }), beforeServiceProgress);


                    // リストア処理
                    this._BackupService.Restore(this.Stack, configuration, path, new Progress<ProgressReportsModel>(
                        progressReport =>
                        {
                            foreach (var p in progressReport.Progresses)
                            {
                                var progressItemModel = mainTaskReports.FirstOrDefault(model => model.Key.Equals(p.Key));
                                if (progressItemModel == null)
                                    continue;

                                progressItemModel.Progress = p.Progress;
                                progressItemModel.ErrorMessages = p.ErrorMessages;
                            }
                        }));

                    // サービスの開始
                    var afterTargets = new[]
                    {
                        new { Reports = apacheServices,       RequiredStart = true },
                        new { Reports = mySqlServices,        RequiredStart = true },
                        new { Reports = subversionServices,   RequiredStart = true },
                        new { Reports = redmineServices,      RequiredStart = true },
                    };


                    var afterServiceProgress = new Progress<ProgressReportsModel>(progressReport =>
                    {
                        foreach (var p in progressReport.Progresses)
                        {
                            var progressItemModel = afterServiceReports.FirstOrDefault(model => model.Key.Equals(p.Key));
                            if (progressItemModel == null)
                                continue;

                            progressItemModel.Progress = p.Progress;
                            progressItemModel.ErrorMessages = p.ErrorMessages;
                        }
                    });

                    foreach (var t in afterTargets)
                        foreach (var s in t.Reports)
                            if (t.RequiredStart)
                                this._BitnamiRedmineService.StartService(s.Key, new ProgressReportsModel(this._DispatcherService, new[] { s.Value }), afterServiceProgress);
                            else
                                this._BitnamiRedmineService.StopService(s.Key, new ProgressReportsModel(this._DispatcherService, new[] { s.Value }), afterServiceProgress);
                };

                progressDialogService.Report = report;
                await progressDialogService.ShowMessage(null, null);
                if (progressDialogService.Result == MessageBoxResult.Cancel)
                {
                    message = Resources.Msg_RestoreCancel;
                }
                else
                {
                    message = Resources.Msg_BackupComplete;
                }
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