using System.Globalization;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using RedArmory.Models;

namespace RedArmory.ViewModels
{

    public class MainViewModel : ViewModelBase
    {

        #region イベント
        #endregion

        #region フィールド
        #endregion

        #region コンストラクタ

        public MainViewModel()
        {
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

        #region オーバーライド
        #endregion

        #region イベントハンドラ

        private void CurrentOnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            //FrameworkElement.LanguageProperty.OverrideMetadata(
            //  typeof(FrameworkElement),
            //    new FrameworkPropertyMetadata(
            //      XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            Properties.Resources.Culture = CultureInfo.GetCultureInfo("ja-JP");
        }

        private static void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
        }

        private static void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
        }

        private static void CurrentOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
        }

        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }
}