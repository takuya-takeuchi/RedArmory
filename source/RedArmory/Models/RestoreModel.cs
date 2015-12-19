using System;
using RedArmory.Models.Configurations;
using RedArmory.Models.Services;
using RedArmory.Models.Services.Dialog;
using RedArmory.Properties;

namespace RedArmory.Models
{

    public sealed class RestoreModel : BackupRestoreModel
    {

        #region フィールド

        private readonly BitNamiRedmineStackConfiguration _Configuration;

        #endregion

        #region コンストラクタ

        public RestoreModel(BitNamiRedmineStack stack)
            : base(stack)
        {
            this._Configuration = ConfigurationService.Instance.GetBitNamiRedmineStackConfiguration(stack.DisplayVersion);
            this.Directory = this._Configuration.DefaultSource;

            this.UpdateState();
        }

        #endregion

        #region プロパティ
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
                progressDialogService.Action = () => BackupService.Instance.Restore(this.Stack, configuration, path, new Progress<BackupRestoreProgressReport>());
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
                    this._Configuration.DefaultSource = this.Directory;

                    this.UpdateState();
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