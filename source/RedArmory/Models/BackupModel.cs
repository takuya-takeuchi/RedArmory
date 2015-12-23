using System;
using System.Collections.ObjectModel;
using System.IO;
using RedArmory.Models.Configurations;
using RedArmory.Models.Services;
using RedArmory.Models.Services.Dialog;
using RedArmory.Properties;

namespace RedArmory.Models
{

    public sealed class BackupModel : BackupRestoreModel
    {

        #region フィールド

        private readonly BitNamiRedmineStackConfiguration _Configuration;

        #endregion

        #region コンストラクタ

        public BackupModel(BitNamiRedmineStack stack)
            : base(stack)
        {
            this._Configuration = ConfigurationService.Instance.GetBitNamiRedmineStackConfiguration(stack.DisplayVersion);
            this.Directory = this._Configuration.DefaultDestionation;

            this.UpdateDiskSpace();
        }

        #endregion

        #region プロパティ

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

            try
            {
                var path = this.Directory;

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
            }
            catch (Exception)
            {
                message = Resources.Msg_BackupFailed;
            }

            var dialogService = new OKDialogService();
            await dialogService.ShowMessage(message, null);
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