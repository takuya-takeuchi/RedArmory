using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;

namespace RedArmory.Models.Services
{
    internal sealed class MySqlService
    {

        #region イベント
        #endregion

        #region フィールド

        private const string MySqlDumpPath = @"mysql\bin\mysqldump.exe";

        #endregion

        #region コンストラクタ

        static MySqlService()
        {
        }

        private MySqlService()
        {
        }

        #endregion

        #region プロパティ

        private static MySqlService _Instance;

        public static MySqlService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new MySqlService());
            }
        }

        #endregion

        #region メソッド

        public void Backup(BitnamiRedmineStack stack, DatabaseConfiguration configuration, string path)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            var apppath = CreateMySqlDumpLocation(stack);
            if (!File.Exists(apppath))
            {
                throw new FileNotFoundException("mysqldump.exe が存在しません。", apppath);
            }

            // サポートしている文字コードは
            // mysql> show character set; を実行
            Encoding encoding;
            switch (configuration.Encoding)
            {
                case "utf8":
                    encoding = Encoding.UTF8;
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} はサポートしていません。", configuration.Encoding));
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

            using (var process = Process.Start(psInfo))
            {
                var contents = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                File.WriteAllText(path, contents, encoding);
            }
        }

        public void Restore(BitnamiRedmineStack stack, DatabaseConfiguration configuration, string path)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("インポートする sql ファイルが存在しません。", path);
            }

            var connectionString = CreateConnectionString(configuration);
            using (var con = new MySqlConnection(connectionString))
            {
                var text = File.ReadAllText(path);
                var script = new MySqlScript(con, text);
                var result = script.Execute();
            }
        }

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

        #endregion

        #endregion

    }
}
