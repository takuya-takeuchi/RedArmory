using System;
using System.IO;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using Mono.Options;
using Ouranos.RedArmory.Interfaces;
using Ouranos.RedArmory.Interop;
using Ouranos.RedArmory.Models.Helpers;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        #region フィールド

        private const int Success = 0;

        private const int ArgumentError = -1;

        #endregion

        #region メソッド

        #region オーバーライド

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string mode = "";
            string version = "";
            string output = "";
            string name = "";
            bool theme = false;
            bool database = false;
            bool plugin = false;
            bool attach = false;

            var p = new OptionSet()
            {
                // 動作モード
                {"m=|mode=", "Mode of application. If omit this argument, GUI application launches. User can specify 'backup' or 'restore'.", v => mode = v},
                
                // バックアップ先ディレクトリ
                {"v=|version=", "Target Redmine version for backup/restore mode.", v => version = v},

                // バックアップ先ディレクトリ
                {"o=|output=", "Output directory for backup mode.", v => output = v},
                
                // バックアップ先ディレクトリの名前
                {"n=|name=", "Output directory name for backup mode. User can select place holder.", v => name = v},
                
                // 復元元ディレクトリ
                {"s=|source=", "Input directory for restore mode.", v => output = v},

                // テーマをバックアップまたは復元の対象にするかどうか
                {"t|theme", "show help.", v => theme = v != null},
                
                // データベースをバックアップまたは復元の対象にするかどうか
                {"d|database", "show help.", v => database = v != null},
                
                // プラグインをバックアップまたは復元の対象にするかどうか
                {"p|plugin", "show help.", v => plugin = v != null},
                
                // 添付ファイルをバックアップまたは復元の対象にするかどうか
                {"a|attach", "show help.", v => attach = v != null}
            };

            string[] args = Environment.GetCommandLineArgs();
            var extra = p.Parse(args); // extra には処理されなかった引数が入る

            if (string.IsNullOrWhiteSpace(mode))
            {
                // GUI で起動
                return;
            }

            // コマンドを実行
            var exitCode = this.ExecuteCommandLine(mode,
                                                   version,
                                                   new BackupConfiguration
                                                   {
                                                       Database = database,
                                                       Files = attach,
                                                       Plugins = plugin,
                                                       Themes = theme
                                                   },
                                                   output,
                                                   name);

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Application.Current.Shutdown(exitCode);
        }

        #endregion

        #region ヘルパーメソッド

        private int ExecuteCommandLine(string mode, string version, BackupConfiguration configuration, string output, string name)
        {
            // インスタンス生成時点で IoC コンテナを生成し、必要なインジェクションが
            // 終了している
            var locator = new ViewModelLocator();
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            try
            {
                // 自プロセスを親プロセスのコンソールにアタッチ
                // とりあえず失敗しない前提
                SafeNativeMethods.AttachConsole(uint.MaxValue);

                // stdoutのストリームを取得
                var defaultStdout = new IntPtr(7);
                var currentStdout = SafeNativeMethods.GetStdHandle(SafeNativeMethods.STD_OUTPUT_HANDLE);

                // リセット
                if (currentStdout != defaultStdout)
                {
                    SafeNativeMethods.SetStdHandle(SafeNativeMethods.STD_OUTPUT_HANDLE, defaultStdout);
                }

                // これ以降は、普通に Console.WriteLine 等が使える
                var writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                Console.SetOut(writer);

                var redmineStack = bitnamiRedmineService.GetBitnamiRedmineStacks().
                    FirstOrDefault(stack => stack.DisplayVersion.Equals(version));

                // 指定した引数のバージョンの Redmine が見つからなかった
                if (redmineStack == null)
                {
                    Console.WriteLine($"Specified Redmine version '{version}' is not found.");
                    return ArgumentError;
                }

                switch (mode.ToLowerInvariant())
                {
                    case "backup":
                        {
                            // フォルダが存在しない場合もあるので作成
                            output = Utility.GetSanitizedDirectoryPath(redmineStack, Path.Combine(output, name));
                            if (!Directory.Exists(output))
                            {
                                Directory.CreateDirectory(output);
                            }

                            Console.WriteLine($"Start backup to '{output}'...");

                            var engine = new BackupEngine(bitnamiRedmineService,
                                                          SimpleIoc.Default.GetInstance<IBackupService>(),
                                                          null,
                                                          SimpleIoc.Default.GetInstance<ILogService>(),
                                                          configuration,
                                                          redmineStack,
                                                          output);

                            var report = engine.PrepareBackup();
                            engine.ExecuteBackup();

                            // プログレスの変更のイベントをサブスクライブしてコンソールに進捗状況を表示する?
                            //progressDialogService.Action = () =>
                            //{
                            //};
                        }
                        return Success;
                    case "restore":
                        {
                            Console.WriteLine($"Start restore from '{output}'...");

                            var engine = new RestoreEngine(SimpleIoc.Default.GetInstance<IBitnamiRedmineService>(),
                                                           SimpleIoc.Default.GetInstance<IBackupService>(),
                                                           null,
                                                           SimpleIoc.Default.GetInstance<ILogService>(),
                                                           configuration,
                                                           redmineStack,
                                                           output);

                            var report = engine.PrepareRestore();
                            engine.ExecuteRestore();

                            // プログレスの変更のイベントをサブスクライブしてコンソールに進捗状況を表示する?
                            //progressDialogService.Action = () =>
                            //{
                            //};
                        }
                        return Success;
                    default:
                        Console.WriteLine($"Argument '{mode}' is invalid.");
                        return ArgumentError;
                }
            }
            finally
            {
                SafeNativeMethods.FreeConsole();
            }
        }

        //static void Main()
        //{
        //    Console.CursorVisible = false;

        //    char[] bars =
        //    {
        //        '／', '―', '＼', '｜'
        //    };

        //    for (int i = 0; i < 100; i++)
        //    {
        //        // 回転する棒を表示
        //        Console.Write(bars[i % 4]);

        //        // 進むパーセンテージを表示
        //        Console.Write("{0, 4:d0}%", i + 1);

        //        // カーソル位置を初期化
        //        Console.SetCursorPosition(0, Console.CursorTop);

        //        // （進行が見えるように）処理を100ミリ秒間休止
        //        System.Threading.Thread.Sleep(100);
        //    }

        //    Console.CursorVisible = true;

        //    // テスト用にプログラムの実行を中断
        //    Console.ReadLine();
        //}

        #endregion

        #endregion

    }
}
