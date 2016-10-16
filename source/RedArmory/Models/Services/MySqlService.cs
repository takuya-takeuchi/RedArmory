using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;

namespace Ouranos.RedArmory.Models.Services
{

    internal sealed class MySqlService : IDatabaseService
    {

        #region イベント
        #endregion

        #region フィールド

        private const string MySqlDumpPath = @"mysql\bin\mysqldump.exe";

        private const string MySqlPath = @"mysql\bin\mysql.exe";

        private readonly ILoggerService _LoggerService;

        #endregion

        #region コンストラクタ

        public MySqlService(ILoggerService loggerService)
        {
            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            this._LoggerService = loggerService;
        }

        #endregion

        #region メソッド

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド

        private static string CreateConnectionString(DatabaseConfiguration configuration)
        {
            return $"userid={configuration.Username};password={configuration.Password};server=localhost;database={configuration.Name};Port={configuration.Port}";
        }

        private static string CreateMySqlDumpLocation(BitnamiRedmineStack stack)
        {
            return Path.Combine(stack.InstallLocation, MySqlDumpPath);
        }

        private static string CreateMySqlLocation(BitnamiRedmineStack stack)
        {
            return Path.Combine(stack.InstallLocation, MySqlPath);
        }

        #endregion

        #endregion

        #region IDatabaseService メンバー

        public void Backup(BitnamiRedmineStack stack, DatabaseConfiguration configuration, string path)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var apppath = CreateMySqlDumpLocation(stack);
            if (!File.Exists(apppath))
                throw new FileNotFoundException("mysqldump.exe が存在しません。", apppath);

            // サポートしている文字コードは
            // mysql> show character set; を実行
            Encoding encoding;
            switch (configuration.Encoding)
            {
                case "utf8":
                    encoding = Encoding.UTF8;
                    break;
                default:
                    throw new NotSupportedException($"{configuration.Encoding} はサポートしていません。");
            }

            const string format =
                "--default-character-set={0} --user={1} --password={2} --port={3} --databases {4}";
            var arguments = string.Format(
                    format,
                    configuration.Encoding,
                    configuration.Username,
                    configuration.Password,
                    configuration.Port,
                    configuration.Name);

            var psInfo = new ProcessStartInfo();
            psInfo.FileName = apppath;
            psInfo.Arguments = arguments;
            psInfo.CreateNoWindow = true;
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardOutput = true;
            psInfo.StandardOutputEncoding = encoding;

            this._LoggerService.Info("Create mysqldump.exe process");
            using (var process = Process.Start(psInfo))
            {
                var contents = process.StandardOutput.ReadToEnd();

                this._LoggerService.Info("Start mysqldump.exe");
                process.WaitForExit();

                this._LoggerService.Info("End mysqldump.exe");
                File.WriteAllText(path, contents, encoding);
            }
        }

        public IEnumerable<EnumerationItem> GetEnumerationItems(BitnamiRedmineStack stack, DatabaseConfiguration configuration)
        {
            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var apppath = CreateMySqlLocation(stack);
            if (!File.Exists(apppath))
                throw new FileNotFoundException("mysql.exe が存在しません。", apppath);

            // サポートしている文字コードは
            // mysql> show character set; を実行
            Encoding encoding;
            switch (configuration.Encoding)
            {
                case "utf8":
                    encoding = Encoding.UTF8;
                    break;
                default:
                    throw new NotSupportedException($"{configuration.Encoding} はサポートしていません。");
            }

            const string format =
                "--default-character-set={0} --user={1} --password={2} --port={3} --databases {4}";
            var arguments = string.Format(
                    format,
                    configuration.Encoding,
                    configuration.Username,
                    configuration.Password,
                    configuration.Port,
                    configuration.Name);

            var psInfo = new ProcessStartInfo();
            psInfo.FileName = apppath;
            psInfo.Arguments = arguments;
            psInfo.CreateNoWindow = true;
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardOutput = true;
            psInfo.StandardOutputEncoding = encoding;

            this._LoggerService.Info("Create mysqldump.exe process");
            using (var process = Process.Start(psInfo))
            {
                var contents = process.StandardOutput.ReadToEnd();

                this._LoggerService.Info("Start mysqldump.exe");
                process.WaitForExit();

                this._LoggerService.Info("End mysqldump.exe");
                //File.WriteAllText(path, contents, encoding);
            }

            return null;
        }

        public void Restore(BitnamiRedmineStack stack, DatabaseConfiguration configuration, string path)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (!File.Exists(path))
                throw new FileNotFoundException("インポートする sql ファイルが存在しません。", path);

            var connectionString = CreateConnectionString(configuration);

            this._LoggerService.Info("Create MySqlConnection");
            using (var con = new MySqlConnection(connectionString))
            {
                var text = File.ReadAllText(path);
                var script = new MySqlScript(con, text);

                this._LoggerService.Info("Execute MySqlScript");
                var result = script.Execute();

                this._LoggerService.Info("MySqlScript,Execute returns {0}", result);
            }
        }

        #endregion

    }

}