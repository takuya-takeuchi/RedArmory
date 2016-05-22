using System.Globalization;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public class MainViewModel : ViewModelBase
    {

        #region イベント
        #endregion

        #region フィールド

        private readonly ILoggerService _LoggerService;

        #endregion

        #region コンストラクタ

        public MainViewModel(ILoggerService loggerService)
        {
            this._LoggerService = loggerService;

            this.Title = AssemblyProperty.Title;

            Application.Current.Startup += CurrentOnStartup;
            Application.Current.Exit += CurrentOnExit;
            Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            Application.Current.LoadCompleted += CurrentOnLoadCompleted;
        }

        #endregion

        #region プロパティ

        private string _Title;

        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        #region イベントハンドラ

        private void CurrentOnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            this._LoggerService.Info($"CurrentOnStartup. startupEventArgs.Args is {string.Join(", ", startupEventArgs.Args)}");

            //FrameworkElement.LanguageProperty.OverrideMetadata(
            //  typeof(FrameworkElement),
            //    new FrameworkPropertyMetadata(
            //      XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            Properties.Resources.Culture = CultureInfo.GetCultureInfo("ja-JP");
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            this._LoggerService.Error($"CurrentOnDispatcherUnhandledException. dispatcherUnhandledExceptionEventArgs.Exception is {dispatcherUnhandledExceptionEventArgs.Exception}");
        }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            this._LoggerService.Info($"CurrentOnExit. exitEventArgs.ApplicationExitCode is {exitEventArgs.ApplicationExitCode}");
        }

        private void CurrentOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            this._LoggerService.Info("CurrentOnLoadCompleted");
        }

        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }
}