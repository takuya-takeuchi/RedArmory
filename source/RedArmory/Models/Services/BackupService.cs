using System;
using System.IO;
using System.Linq;

namespace RedArmory.Models.Services
{

    internal sealed class BackupService
    {

        #region フィールド

        private static readonly string FilesDirectoryName = "files";

        private static readonly string PluginsDirectoryName = "plugins";

        private static readonly string ThemeseDirectoryName = "themes";

        #endregion

        #region コンストラクタ

        static BackupService()
        {
        }

        private BackupService()
        {
        }

        #endregion

        #region プロパティ

        private static BackupService _Instance;

        public static BackupService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new BackupService());
            }
        }

        #endregion

        #region メソッド

        public void Backup(BitNamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<BackupRestoreProgressReport> progress)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format("{0} は存在しません。", path));
            }

            var report = new BackupRestoreProgressReport();

            // データベースのバックアップ
            if (configuration.Database)
            {
                report.Database = ProgressState.InProgress;
                progress.Report(report);

                var databaseConfigurations = RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                foreach (var databaseConfiguration in databaseConfigurations)
                {
                    var sqlFileName = string.Format("{0}.sql", databaseConfiguration.Mode);
                    var filepath = Path.Combine(path, sqlFileName);

                    MySqlService.Instance.Backup(stack, databaseConfiguration, filepath);
                }

                report.Database = ProgressState.Complete;
                progress.Report(report);
            }
            else
            {
                report.Database = ProgressState.NotRequire;
            }

            // プラグイン、テーマ、添付ファイルのバックアップ
            var rules = new[]
            {
                new
                {
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
                    continue;
                }

                var sourceDir = Path.Combine(stack.InstallLocation, rule.Source);
                var targetDir = Path.Combine(path, rule.Target);

                rule.ReportAction(ProgressState.InProgress);

                CopyDirectory(sourceDir, targetDir);

                rule.ReportAction(ProgressState.Complete);
            }
        }

        public BackupConfiguration CheckRestoreFolder(BitNamiRedmineStack stack, string path)
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

        public void Restore(BitNamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<BackupRestoreProgressReport> progress)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format("{0} は存在しません。", path));
            }

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

                    MySqlService.Instance.Restore(stack, databaseConfiguration, sqlFilePath);
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

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド

        private static void CopyDirectory(string sourceDir, string targetDir)
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
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                try
                {
                    File.Copy(file, targetDir + Path.GetFileName(file), true);
                }
                catch (Exception ex)
                {

                }
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

    }

}
