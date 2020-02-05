using System.Globalization;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    internal class MainViewModel : ViewModelBase
    {

        #region Events
        #endregion

        #region Fields

        private readonly ILogService _LogService;

        #endregion

        #region Constructors

        public MainViewModel(ILogService logService)
        {
            this._LogService = logService;

            this.Title = AssemblyProperty.Title;

            Application.Current.Startup += this.CurrentOnStartup;
            Application.Current.Exit += this.CurrentOnExit;
            Application.Current.DispatcherUnhandledException += this.CurrentOnDispatcherUnhandledException;
            Application.Current.LoadCompleted += this.CurrentOnLoadCompleted;
        }

        #endregion

        #region Properties

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

        #region Methods

        #region Event Handlers

        private void CurrentOnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            this._LogService.Info($"CurrentOnStartup. startupEventArgs.Args is {string.Join(", ", startupEventArgs.Args)}");

            //FrameworkElement.LanguageProperty.OverrideMetadata(
            //  typeof(FrameworkElement),
            //    new FrameworkPropertyMetadata(
            //      XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            Properties.Resources.Culture = CultureInfo.GetCultureInfo("ja-JP");
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            this._LogService.Error($"CurrentOnDispatcherUnhandledException. dispatcherUnhandledExceptionEventArgs.Exception is {dispatcherUnhandledExceptionEventArgs.Exception}");
        }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            this._LogService.Info($"CurrentOnExit. exitEventArgs.ApplicationExitCode is {exitEventArgs.ApplicationExitCode}");
        }

        private void CurrentOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            this._LogService.Info("CurrentOnLoadCompleted");
        }

        #endregion

        #region Helpers
        #endregion

        #endregion

    }
}