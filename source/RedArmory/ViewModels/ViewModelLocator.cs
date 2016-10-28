/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:RedArmory"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    internal class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            
            if (!SimpleIoc.Default.IsRegistered<System.Windows.Threading.Dispatcher>())
                SimpleIoc.Default.Register(() => System.Windows.Application.Current.Dispatcher);

            Register<ILoggerService, LoggerService>();
            Register<IDispatcherService, DispatcherService>();
            Register<IApplicationSettingService, ApplicationSettingService>();
            Register<IRedmineDatabaseConfigurationService, RedmineDatabaseConfigurationService>();
            Register<IDatabaseService, MySqlService>();
            Register<IBackupService, BackupService>();
            Register<IBitnamiRedmineService, BitnamiRedmineService>();
            Register<ITaskService, TaskService>();

            Register<MainViewModel>();
            Register<AboutViewModel>();
            Register<BackupViewModel>();
            Register<GeneralViewModel>();
            Register<RestoreViewModel>();
            Register<SettingViewModel>();
        }

        private static void Register<T>(System.Func<T> factory)
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
                SimpleIoc.Default.Register(() => factory);
        }

        private static void Register<T>()
            where T : class
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
                SimpleIoc.Default.Register<T>();
        }

        private static void Register<T, U>()
            where T : class
            where U : class
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
                SimpleIoc.Default.Register<T, U>();
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public BackupViewModel Backups
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BackupViewModel>();
            }
        }

        public GeneralViewModel General
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GeneralViewModel>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public RestoreViewModel Restore
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RestoreViewModel>();
            }
        }

        public SettingViewModel Setting
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}