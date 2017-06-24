using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Ouranos.RedArmory.Interfaces;
using Ouranos.RedArmory.Models.Helpers;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.Models.Services.Dialog;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Models
{

    internal sealed class BackupModel : BackupRestoreModel
    {

        #region コンストラクタ

        public BackupModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService,
            IDialogService dialogService,
            ILogService logService,
            BitnamiRedmineStack stack)
            : base(applicationSettingService, bitnamiRedmineService, backupService, dispatcherService, dialogService, logService, stack)
        {
            // Apply Setting
            RedmineSetting redmineSetting;
            this.GetApplicationSetting(out redmineSetting);
            this.Directory = redmineSetting.Backup.BaseDirectory;
            this.DirectoryName = redmineSetting.Backup.DirectoryName;
            this.Database = redmineSetting.Backup.Database;
            this.Files = redmineSetting.Backup.Files;
            this.Plugins = redmineSetting.Backup.Plugins;
            this.Themes = redmineSetting.Backup.Themes;

            this.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "Directory":
                        var path = Path.Combine(this.Directory, this.DirectoryName);
                        this.OutputDirectory = Utility.GetSanitizedDirectoryPath(stack, path);
                        break;
                }
            };

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
                
                var path = Path.Combine(this.Directory, this.DirectoryName);
                this.OutputDirectory = Utility.GetSanitizedDirectoryPath(this.Stack, path);
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
                await this._DialogService.ShowMessage(MessageBoxButton.OK, Resources.Msg_BackupFailed, null);
                return;
            }

            // 空かどうか検証
            if (!this.IsOutputDirectoryEmpty(directory.FullName))
            {
                var result = await this._DialogService.ShowMessage(MessageBoxButton.YesNo, Resources.Msg_DirectoryIsNotEmpty, null);
                if (result == MessageBoxResult.No)
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
                var progressDialogService = new ProgressDialogService { IsAutoClose = true };

                var engine = new BackupEngine(this._BitnamiRedmineService,
                                        this._BackupService,
                                        this._DispatcherService,
                                        this._LogService,
                                        configuration,
                                        this.Stack,
                                        this._OutputDirectory);

                var report = engine.PrepareBackup();
                progressDialogService.Action = () =>
                {
                    engine.ExecuteBackup();
                };

                progressDialogService.Report = report;
                await progressDialogService.ShowMessage(null, null);
                if (progressDialogService.Result == MessageBoxResult.Cancel)
                {
                    message = Resources.Msg_BackupCancel;
                    await this._DialogService.ShowMessage(MessageBoxButton.OK, message, null);

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
                this._LogService.Error(message);

                message = $"StackTrace is {ex.StackTrace}";
                this._LogService.Error(message);

                message = Resources.Msg_BackupFailed;
                await this._DialogService.ShowMessage(MessageBoxButton.OK, message, null);

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

            await this._DialogService.ShowMessage(MessageBoxButton.OK, message, null);
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

        #endregion

        #endregion

    }

}