using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using RedArmory.Extensions;
using RedArmory.Models.Services;
using RedArmory.Models.Services.Dialog;
using RedArmory.Properties;

namespace RedArmory.Models
{

    public sealed class BackupModel : BackupRestoreModel
    {

        #region コンストラクタ

        public BackupModel(
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
                        this.UpdateOutputDirectory();
                        break;
                }
            };

            // Apply Setting
            RedmineSetting redmineSetting;
            this.GetApplicationSetting(out redmineSetting);
            this.Directory = redmineSetting.Backup.BaseDirectory;
            this.DirectoryName = redmineSetting.Backup.DirectoryName;
            this.Database = redmineSetting.Backup.Database;
            this.Files = redmineSetting.Backup.Files;
            this.Plugins = redmineSetting.Backup.Plugins;
            this.Themes = redmineSetting.Backup.Themes;

            this.UpdateDiskSpace();
        }

        #endregion

        #region プロパティ

        private string _DirectoryName;

        public string DirectoryName
        {
            get
            {
                return this._DirectoryName;
            }
            set
            {
                this._DirectoryName = value;
                this.RaisePropertyChanged();

                this.UpdateOutputDirectory();
            }
        }

        private ObservableCollection<DiskInfo> _DriveSpaces;

        public ObservableCollection<DiskInfo> DriveSpaces
        {
            get
            {
                return this._DriveSpaces;
            }
            set
            {
                this._DriveSpaces = value;
                this.RaisePropertyChanged();
            }
        }

        private string _OutputDirectory;

        public string OutputDirectory
        {
            get
            {
                return this._OutputDirectory;
            }
            private set
            {
                this._OutputDirectory = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        protected override bool CanExecute()
        {
            var canAction = this.Database;
            canAction |= this.Themes;
            canAction |= this.Plugins;
            canAction |= this.Files;

            return canAction && this.Stack != null &&
                   !string.IsNullOrWhiteSpace(this.Directory) &&
                   System.IO.Directory.Exists(this.Directory);
        }

        protected override bool CanExecuteSelectDirectory()
        {
            return true;
        }

        protected override async void Execute()
        {
            string message;

            var path = this.OutputDirectory;
            DirectoryInfo directory;

            // 出力先ディレクトリのディレクトリ名の検証
            try
            {
                directory = System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                message = Resources.Msg_BackupFailed;
                await new OKDialogService().ShowMessage(message, null);
                return;
            }

            // 空かどうか検証
            if (!IsOutputDirectoryEmpty(directory.FullName))
            {
                message = Resources.Msg_DirectoryIsNotEmpty;
                var yesNoDialogService = new YesNoDialogService();
                await yesNoDialogService.ShowMessage(message, null);

                if (yesNoDialogService.Result == MessageBoxResult.No)
                {
                    return;
                }
            }

            var configuration = new BackupConfiguration
            {
                Database = this.Database,
                Files = this.Files,
                Plugins = this.Plugins,
                Themes = this.Themes
            };

            try
            {
                var progressDialogService = new ProgressDialogService {IsAutoClose = true};

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


                    // バックアップ処理
                    this._BackupService.Backup(this.Stack, configuration, path, new Progress<ProgressReportsModel>(
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
                    message = Resources.Msg_BackupCancel;
                    await new OKDialogService().ShowMessage(message, null);

                    return;
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
                await new OKDialogService().ShowMessage(message, null);

                return;
            }


            // Update Setting
            RedmineSetting redmineSetting;
            var applicationSetting = this.GetApplicationSetting(out redmineSetting);
            redmineSetting.Backup.Database = configuration.Database;
            redmineSetting.Backup.Files = configuration.Files;
            redmineSetting.Backup.Plugins = configuration.Plugins;
            redmineSetting.Backup.Themes = configuration.Themes;
            redmineSetting.Backup.BaseDirectory = this.Directory;
            redmineSetting.Backup.DirectoryName = this.DirectoryName;

            var history = new BackupHistorySetting
            {
                DisplayVersion = this.Stack.DisplayVersion,
                DateTime = DateTime.UtcNow,
                OutputDirectory = path
            };
            this._ApplicationSettingService.BackupHistories.Add(history);
            applicationSetting.BackupHistories.Add(history);

            this._ApplicationSettingService.UpdateApplicationSetting(applicationSetting);

            await new OKDialogService().ShowMessage(message, null);
        }

        protected override void ExecuteSelectDirectory()
        {
            using (var dlg = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog())
            {
                dlg.Title = Resources.Word_SelectTargetDirectoryTitle;
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
                    this.UpdateDiskSpace();

                    this.RaiseCanExecuteBackupRestoreChanged();
                }
            }
        }

        #region ヘルパーメソッド

        private bool IsOutputDirectoryEmpty(string path)
        {
            try
            {
                var entries = System.IO.Directory.GetFileSystemEntries(path);
                return entries.Length == 0;
            }
            catch
            {
                // アクセス権がないなどの場合は空でないとする
                return false;
            }
        }

        private void UpdateDiskSpace()
        {
            var isEmpty = true;
            var spaces = new ObservableCollection<DiskInfo>();

            try
            {
                var destination = this.Directory;
                if (string.IsNullOrWhiteSpace(destination))
                {
                    return;
                }

                var driveName = Path.GetPathRoot(destination);
                var drive = new DriveInfo(driveName);
                if (!drive.IsReady)
                {
                    return;
                }

                var available = (double)drive.AvailableFreeSpace;
                var usage = drive.TotalSize - available;

                // MB へ変換
                const double coefficient = 1024 * 1024;
                usage /= coefficient;
                available /= coefficient;

                spaces.Add(new DiskInfo { Category = Resources.Word_UsedSpace, Number = (long)usage });
                spaces.Add(new DiskInfo { Category = Resources.Word_AvailableSpace, Number = (long)available });
                isEmpty = false;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (isEmpty)
                {
                    spaces.Add(new DiskInfo { Category = Resources.Word_UnknownSpace, Number = 100 });
                }

                this.DriveSpaces = spaces;
            }
        }

        private void UpdateOutputDirectory()
        {
            var keywords = new[]
            {
                "%VERSION%",
                "%LONGDATE%",
                "%SHORTDATE%",
            };

            var name = this.DirectoryName ?? "";
            var datetime = DateTime.Now;

            foreach (var keyword in keywords)
            {
                do
                {
                    if (name.IndexOf(keyword, StringComparison.InvariantCulture) == -1)
                    {
                        break;
                    }

                    switch (keyword)
                    {
                        case "%VERSION%":
                            name = name.Replace(keyword, this.Stack.DisplayVersion);
                            break;
                        case "%LONGDATE%":
                            name = name.Replace(keyword, datetime.ToString("yyyyMMdd hhmmss"));
                            break;
                        case "%SHORTDATE%":
                            name = name.Replace(keyword, datetime.ToString("yyyyMMdd"));
                            break;
                    }
                } while (true);
            }

            this.OutputDirectory = Path.Combine(this.Directory ?? "", name);
        }

        #endregion

        #endregion

    }
}