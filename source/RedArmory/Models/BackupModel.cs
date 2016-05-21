using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using RedArmory.Models.Configurations;
using RedArmory.Models.Services;
using RedArmory.Models.Services.Dialog;
using RedArmory.Properties;

namespace RedArmory.Models
{

    public sealed class BackupModel : BackupRestoreModel
    {

        #region フィールド

        private readonly BitnamiRedmineStackConfiguration _Configuration;

        #endregion

        #region コンストラクタ

        public BackupModel(BitnamiRedmineStack stack)
            : base(stack)
        {
            this._Configuration = ConfigurationService.Instance.GetBitnamiRedmineStackConfiguration(stack.DisplayVersion);
            this.Directory = this._Configuration.DefaultDestionation;

            this.UpdateDiskSpace();

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

            try
            {
                var configuration = new BackupConfiguration
                {
                    Database = this.Database,
                    Files = this.Files,
                    Plugins = this.Plugins,
                    Themes = this.Themes
                };

                var progressDialogService = new ProgressDialogService();
                progressDialogService.Action = () => BackupService.Instance.Backup(this.Stack, configuration, path, new Progress<BackupRestoreProgressReport>());
                await progressDialogService.ShowMessage(null, null);

                message = Resources.Msg_BackupComplete;

                // Update Setting
                RedmineSetting redmineSetting;
                var applicationSetting = this.GetApplicationSetting(out redmineSetting);
                redmineSetting.Backup.Database = configuration.Database;
                redmineSetting.Backup.Files = configuration.Files;
                redmineSetting.Backup.Plugins = configuration.Plugins;
                redmineSetting.Backup.Themes = configuration.Themes;
                redmineSetting.Backup.BaseDirectory = this.Directory;
                redmineSetting.Backup.DirectoryName = this.DirectoryName;
                ApplicationSettingService.Instance.UpdateApplicationSetting(applicationSetting);
            }
            catch (Exception)
            {
                message = Resources.Msg_BackupFailed;
            }

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
                    this._Configuration.DefaultDestionation = this.Directory;
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