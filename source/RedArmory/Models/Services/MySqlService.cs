using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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

            var apppath = CreateMySqlDumpLocation(stack);
            if (!File.Exists(apppath))
            {
                throw new FileNotFoundException("mysqldump.exe が存在しません。", apppath);
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("インポートする sql ファイルが存在しません。", path);
            }

            const string format =
                "--user={0} --password={1} --port={2} --databases {3}";
            var arguments = string.Format(
                    format,
                    configuration.Encoding,
                    configuration.Username,
                    configuration.Password,
                    configuration.Port);

            var psInfo = new ProcessStartInfo();
            psInfo.FileName = apppath;
            psInfo.Arguments = arguments;
            psInfo.CreateNoWindow = true;
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardInput = true;

            using (var process = Process.Start(psInfo))
            {
                var text = File.ReadAllText(path);
                process.StandardInput.WriteAsync(text);
                //process.StandardInput.Close();

                process.WaitForExit();
            }
        }

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド

        private static string CreateMySqlDumpLocation(BitnamiRedmineStack stack)
        {
            return Path.Combine(stack.InstallLocation, MySqlDumpPath);
        }

        #endregion

        #endregion

    }
}
