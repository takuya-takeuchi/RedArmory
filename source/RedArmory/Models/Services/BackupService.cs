﻿using System;
using System.IO;
using System.Linq;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Models.Services
{

    internal sealed class BackupService : IBackupService
    {

        #region Fields

        private static readonly string FilesDirectoryName = "files";

        private static readonly string PluginsDirectoryName = "plugins";

        private static readonly string ThemeseDirectoryName = "themes";

        private readonly IDatabaseService _DatabaseService;

        private readonly IRedmineDatabaseConfigurationService _DatabaseConfigurationService;

        private readonly IDispatcherService _DispatcherService;

        private readonly ILogService _LogService;

        #endregion

        #region Constructors

        public BackupService(IDatabaseService databaseService, IRedmineDatabaseConfigurationService databaseConfigurationService, IDispatcherService dispatcherService, ILogService logService)
        {
            if (databaseService == null)
                throw new ArgumentNullException(nameof(databaseService));

            if (databaseConfigurationService == null)
                throw new ArgumentNullException(nameof(databaseConfigurationService));

            if (dispatcherService == null)
                throw new ArgumentNullException(nameof(dispatcherService));

            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            this._DatabaseService = databaseService;
            this._DatabaseConfigurationService = databaseConfigurationService;
            this._DispatcherService = dispatcherService;
            this._LogService = logService;
        }

        #endregion

        #region Methods

        #region Helpers

        private void CopyDirectory(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                File.SetAttributes(targetDir, File.GetAttributes(sourceDir));
            }

            // 末尾のパス区切り文字を全部削除
            targetDir = targetDir.TrimEnd(Path.DirectorySeparatorChar);

            // 末尾に 1 つだけパス区切り文字を追加
            targetDir = targetDir + Path.DirectorySeparatorChar;

            // コピー元のディレクトリにあるファイルをコピー
            Exception exception = null;
            string sourceFileName = null;
            string destFileName = null;

            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                var dest = $"{targetDir}{Path.GetFileName(file)}";

                try
                {
                    File.Copy(file, dest, true);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    sourceFileName = file;
                    destFileName = dest;
                }
            }

            if (exception != null)
            {
                var message = $"Failed to copy '{sourceFileName}' to '{destFileName}'";
                this._LogService.Error(message);
            }

            // コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            var dirs = Directory.GetDirectories(sourceDir);
            foreach (var dir in dirs)
            {
                this.CopyDirectory(dir, targetDir + Path.GetFileName(dir));
            }
        }

        #endregion

        #endregion

        #region IBackupService Members

        public void Backup(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<ProgressReportsModel> progress = null)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"{path} は存在しません。");

            var databaseName = Resources.Word_Database;
            var pluginName = Resources.Word_Plugin;
            var themeName = Resources.Word_Theme;
            var attachedFileName = Resources.Word_AttachedFile;

            var report = new ProgressReportsModel(this._DispatcherService, new[]
            {
                new ProgressItemModel {Key = databaseName, Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = pluginName, Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = themeName, Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = attachedFileName, Progress = ProgressState.NotStart},
            });

            // データベースのバックアップ
            if (configuration.Database)
            {
                report.UpdateProgress(databaseName, ProgressState.InProgress);
                progress?.Report(report);

                var databaseConfigurations = this._DatabaseConfigurationService.GetDatabaseConfiguration(stack).ToArray();
                foreach (var databaseConfiguration in databaseConfigurations)
                {
                    var sqlFileName = $"{databaseConfiguration.Mode}.sql";
                    var filepath = Path.Combine(path, sqlFileName);

                    this._DatabaseService.Backup(stack, databaseConfiguration, filepath);
                }

                report.UpdateProgress(databaseName, ProgressState.Complete);
                progress?.Report(report);
                report.AddErrorMessage(databaseName, Resources.Msg_BackupComplete);
            }
            else
            {
                report.UpdateProgress(databaseName, ProgressState.NotRequire);
                report.AddErrorMessage(databaseName, Resources.Msg_BackupSkip);

                this._LogService.Info("Database is skipped");
            }

            // プラグイン、テーマ、添付ファイルのバックアップ
            var rules = new[]
            {
                new
                {
                    Name = "Plugin",
                    Condition = configuration.Plugins,
                    Source = BackupConfiguration.PluginsPath,
                    Target = PluginsDirectoryName,
                    ReportAction = new Action<ProgressState>(state =>
                    {
                        report.UpdateProgress(pluginName, state);
                        progress?.Report(report);

                        switch (state)
                        {
                            case ProgressState.Complete:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupComplete);
                                break;
                            case ProgressState.NotRequire:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupSkip);
                                break;
                            case ProgressState.Failed:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupFailed);
                                break;
                        }
                    } )
                },
                new
                {
                    Name = "Themes",
                    Condition = configuration.Themes,
                    Source = BackupConfiguration.ThemesePath,
                    Target = ThemeseDirectoryName,
                    ReportAction = new Action<ProgressState>(state =>
                    {
                        report.UpdateProgress(themeName, state);
                        progress?.Report(report);

                        switch (state)
                        {
                            case ProgressState.Complete:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupComplete);
                                break;
                            case ProgressState.NotRequire:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupSkip);
                                break;
                            case ProgressState.Failed:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupFailed);
                                break;
                        }
                    } )
                },
                new
                {
                    Name = "AttachedFile",
                    Condition = configuration.Files,
                    Source = BackupConfiguration.FilesPath,
                    Target = FilesDirectoryName,
                    ReportAction = new Action<ProgressState>(state =>
                    {
                        report.UpdateProgress(attachedFileName, state);
                        progress?.Report(report);

                        switch (state)
                        {
                            case ProgressState.Complete:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupComplete);
                                break;
                            case ProgressState.NotRequire:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupSkip);
                                break;
                            case ProgressState.Failed:
                                report.AddErrorMessage(databaseName, Resources.Msg_BackupFailed);
                                break;
                        }
                    } )
                }
            };

            foreach (var rule in rules)
            {
                if (!rule.Condition)
                {
                    rule.ReportAction(ProgressState.NotRequire);

                    this._LogService.Info($"{rule.Name} is skipped");
                    continue;
                }

                var sourceDir = Path.Combine(stack.InstallLocation, rule.Source);
                var targetDir = Path.Combine(path, rule.Target);

                rule.ReportAction(ProgressState.InProgress);

                this._LogService.Info($"Start copy {rule.Name}");
                this.CopyDirectory(sourceDir, targetDir);
                this._LogService.Info($"End copy {rule.Name}");

                rule.ReportAction(ProgressState.Complete);
            }
        }

        public BackupConfiguration CheckRestoreFolder(BitnamiRedmineStack stack, string path)
        {
            var configuration = new BackupConfiguration();

            // データベース
            var databaseConfigurations = this._DatabaseConfigurationService.GetDatabaseConfiguration(stack).ToArray();
            foreach (var databaseConfiguration in databaseConfigurations)
            {
                var sqlFileName = string.Format("{0}.sql", databaseConfiguration.Mode);
                var sqlFilePath = Path.Combine(path, sqlFileName);
                if (!File.Exists(sqlFilePath))
                {
                    continue;
                }

                configuration.Database = true;
                break;
            }

            // プラグイン、テーマ、添付ファイル
            var rules = new[]
            {
                new
                {
                    Target = BackupConfiguration.PluginsPath,
                    Source = PluginsDirectoryName,
                    CheckAction = new Action(() =>
                    {
                        configuration.Plugins = true;
                    } )
                },
                new
                {
                    Target = BackupConfiguration.ThemesePath,
                    Source = ThemeseDirectoryName,
                    CheckAction = new Action(() =>
                    {
                        configuration.Themes = true;
                    } )
                },
                new
                {
                    Target = BackupConfiguration.FilesPath,
                    Source = FilesDirectoryName,
                    CheckAction = new Action(() =>
                    {
                        configuration.Files = true;
                    } )
                }
            };

            foreach (var rule in rules)
            {
                var sourceDir = Path.Combine(path, rule.Source);
                var condition = Directory.Exists(sourceDir);
                if (!condition)
                {
                    continue;
                }

                rule.CheckAction();
            }

            return configuration;
        }

        public void Restore(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<ProgressReportsModel> progress = null)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"{path} は存在しません。");

            var databaseName = Resources.Word_Database;
            var pluginName = Resources.Word_Plugin;
            var themeName = Resources.Word_Theme;
            var attachedFileName = Resources.Word_AttachedFile;

            var report = new ProgressReportsModel(this._DispatcherService, new[]
            {
                new ProgressItemModel {Key = databaseName, Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = pluginName, Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = themeName, Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = attachedFileName, Progress = ProgressState.NotStart},
            });

            // データベースの復元
            if (configuration.Database)
            {
                report.UpdateProgress(databaseName, ProgressState.InProgress);
                progress?.Report(report);

                var databaseConfigurations = this._DatabaseConfigurationService.GetDatabaseConfiguration(stack).ToArray();
                foreach (var databaseConfiguration in databaseConfigurations)
                {
                    var sqlFileName = $"{databaseConfiguration.Mode}.sql";
                    var sqlFilePath = Path.Combine(path, sqlFileName);
                    if (!File.Exists(sqlFilePath))
                    {
                        continue;
                    }

                    this._DatabaseService.Restore(stack, databaseConfiguration, sqlFilePath);
                }

                report.UpdateProgress(databaseName, ProgressState.Complete);
                progress?.Report(report);
                report.AddErrorMessage(databaseName, Resources.Msg_RestoreComplete);
            }
            else
            {
                report.UpdateProgress(databaseName, ProgressState.NotRequire);
                report.AddErrorMessage(databaseName, Resources.Msg_RestoreSkip);

                this._LogService.Info("Database is skipped");
            }

            // プラグイン、テーマ、添付ファイルの復元
            var rules = new[]
            {
                new
                {
                    Condition = configuration.Plugins,
                    Target = BackupConfiguration.PluginsPath,
                    Source = PluginsDirectoryName,
                    CheckAction = new Action<ProgressState>(state =>
                    {
                        report.UpdateProgress(pluginName, state);
                        progress?.Report(report);

                        switch (state)
                        {
                            case ProgressState.Complete:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreComplete);
                                break;
                            case ProgressState.NotRequire:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreSkip);
                                break;
                            case ProgressState.Failed:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreFailed);
                                break;
                        }
                    } )
                },
                new
                {
                    Condition = configuration.Themes,
                    Target = BackupConfiguration.ThemesePath,
                    Source = ThemeseDirectoryName,
                    CheckAction = new Action<ProgressState>(state =>
                    {
                        report.UpdateProgress(themeName, state);
                        progress?.Report(report);

                        switch (state)
                        {
                            case ProgressState.Complete:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreComplete);
                                break;
                            case ProgressState.NotRequire:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreSkip);
                                break;
                            case ProgressState.Failed:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreFailed);
                                break;
                        }
                    } )
                },
                new
                {
                    Condition = configuration.Files,
                    Target = BackupConfiguration.FilesPath,
                    Source = FilesDirectoryName,
                    CheckAction = new Action<ProgressState>(state =>
                    {
                        report.UpdateProgress(attachedFileName, state);
                        progress?.Report(report);

                        switch (state)
                        {
                            case ProgressState.Complete:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreComplete);
                                break;
                            case ProgressState.NotRequire:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreSkip);
                                break;
                            case ProgressState.Failed:
                                report.AddErrorMessage(databaseName, Resources.Msg_RestoreFailed);
                                break;
                        }
                    })
                }
            };

            foreach (var rule in rules)
            {
                if (!rule.Condition)
                {
                    rule.CheckAction(ProgressState.NotRequire);
                    continue;
                }

                var sourceDir = Path.Combine(path, rule.Source);
                var targetDir = Path.Combine(stack.InstallLocation, rule.Target);
                var condition = Directory.Exists(sourceDir);
                if (!condition)
                {
                    rule.CheckAction(ProgressState.Failed);
                    continue;
                }

                rule.CheckAction(ProgressState.InProgress);

                this.CopyDirectory(sourceDir, targetDir);

                rule.CheckAction(ProgressState.Complete);
            }
        }

        #endregion

    }

}
