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

        #region �C�x���g
        #endregion

        #region �t�B�[���h
        #endregion

        #region �R���X�g���N�^

        public MainViewModel()
        {
            this.Title = AssemblyProperty.Title;

            Application.Current.Startup += CurrentOnStartup;
            Application.Current.Exit += CurrentOnExit;
            Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            Application.Current.LoadCompleted += CurrentOnLoadCompleted;
        }

        #endregion

        #region �v���p�e�B

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

        #region ���\�b�h

        #region �I�[�o�[���C�h
        #endregion

        #region �C�x���g�n���h��

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

        #region �w���p�[���\�b�h
        #endregion

        #endregion

    }
}