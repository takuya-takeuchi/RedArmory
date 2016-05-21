﻿using System;
using System.IO;
using System.Linq;

namespace RedArmory.Models.Services
{

    internal sealed class BackupService : IBackupService
    {

        #region フィールド

        private static readonly string FilesDirectoryName = "files";

        private static readonly string PluginsDirectoryName = "plugins";

        private static readonly string ThemeseDirectoryName = "themes";

        private readonly IDatabaseService _DatabaseService;

        private readonly ILoggerService _LoggerService;

        #endregion

        #region コンストラクタ

        public BackupService(IDatabaseService databaseService, ILoggerService loggerService)
        {
            if (databaseService == null)
                throw new ArgumentNullException(nameof(databaseService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            this._DatabaseService = databaseService;
            this._LoggerService = loggerService;
        }

        #endregion

        #region メソッド

        #region ヘルパーメソッド

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
                this._LoggerService.Error(message);
            }

            // コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            var dirs = Directory.GetDirectories(sourceDir);
            foreach (var dir in dirs)
            {
                CopyDirectory(dir, targetDir + Path.GetFileName(dir));
            }
        }

        #endregion

        #endregion

        #region IBackupService メンバー

        public void Backup(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<BackupRestoreProgressReport> progress)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"{path} は存在しません。");

            var report = new BackupRestoreProgressReport();

            // データベースのバックアップ
            if (configuration.Database)
            {
                report.Database = ProgressState.InProgress;
                progress.Report(report);

                var databaseConfigurations = RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                foreach (var databaseConfiguration in databaseConfigurations)
                {
                    var sqlFileName = $"{databaseConfiguration.Mode}.sql";
                    var filepath = Path.Combine(path, sqlFileName);

                    this._DatabaseService.Backup(stack, databaseConfiguration, filepath);
                }

                report.Database = ProgressState.Complete;
                progress.Report(report);
            }
            else
            {
                report.Database = ProgressState.NotRequire;

                this._LoggerService.Info("Database is skipped");
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
                        report.Plugin = state;
                        progress.Report(report);
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
                        report.Theme = state;
                        progress.Report(report);
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
                        report.AttachedFile = state;
                        progress.Report(report);
                    } )
                }
            };

            foreach (var rule in rules)
            {
                if (!rule.Condition)
                {
                    rule.ReportAction(ProgressState.NotRequire);

                    this._LoggerService.Info($"{rule.Name} is skipped");
                    continue;
                }

                var sourceDir = Path.Combine(stack.InstallLocation, rule.Source);
                var targetDir = Path.Combine(path, rule.Target);

                rule.ReportAction(ProgressState.InProgress);

                this._LoggerService.Info($"Start copy {rule.Name}");
                CopyDirectory(sourceDir, targetDir);
                this._LoggerService.Info($"End copy {rule.Name}");

                rule.ReportAction(ProgressState.Complete);
            }
        }

        public BackupConfiguration CheckRestoreFolder(BitnamiRedmineStack stack, string path)
        {
            var configuration = new BackupConfiguration();

            // データベース
            var databaseConfigurations = RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
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

        public void Restore(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<BackupRestoreProgressReport> progress)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"{path} は存在しません。");

            var report = new BackupRestoreProgressReport();

            // データベースの復元
            if (configuration.Database)
            {
                report.Database = ProgressState.InProgress;
                progress.Report(report);

                var databaseConfigurations = RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                foreach (var databaseConfiguration in databaseConfigurations)
                {
                    var sqlFileName = string.Format("{0}.sql", databaseConfiguration.Mode);
                    var sqlFilePath = Path.Combine(path, sqlFileName);
                    if (!File.Exists(sqlFilePath))
                    {
                        continue;
                    }

                    this._DatabaseService.Restore(stack, databaseConfiguration, sqlFilePath);
                }

                report.Database = ProgressState.Complete;
                progress.Report(report);
            }
            else
            {
                report.Database = ProgressState.NotRequire;
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
                        report.Plugin = state;
                        progress.Report(report);
                    } )
                },
                new
                {
                    Condition = configuration.Themes,
                    Target = BackupConfiguration.ThemesePath,
                    Source = ThemeseDirectoryName,
                    CheckAction = new Action<ProgressState>(state =>
                    {
                        report.Theme = state;
                        progress.Report(report);
                    } )
                },
                new
                {
                    Condition = configuration.Files,
                    Target = BackupConfiguration.FilesPath,
                    Source = FilesDirectoryName,
                    CheckAction = new Action<ProgressState>(state =>
                    {
                        report.AttachedFile = state;
                        progress.Report(report);
                    } )
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

                CopyDirectory(sourceDir, targetDir);

                rule.CheckAction(ProgressState.Complete);
            }
        }

        #endregion

    }

}
