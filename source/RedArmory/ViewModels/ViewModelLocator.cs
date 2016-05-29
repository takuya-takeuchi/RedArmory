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

using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
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

            SimpleIoc.Default.Register(() => System.Windows.Application.Current.Dispatcher);

            SimpleIoc.Default.Register<ILoggerService, LoggerService>();
            SimpleIoc.Default.Register<IDispatcherService, DispatcherService>();
            SimpleIoc.Default.Register<IApplicationSettingService, ApplicationSettingService>();
            SimpleIoc.Default.Register<IRedmineDatabaseConfigurationService, RedmineDatabaseConfigurationService>();
            SimpleIoc.Default.Register<IDatabaseService, MySqlService>();
            SimpleIoc.Default.Register<IBackupService, BackupService>();
            SimpleIoc.Default.Register<IBitnamiRedmineService, BitnamiRedmineService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<BackupViewModel>();
            SimpleIoc.Default.Register<GeneralViewModel>();
            SimpleIoc.Default.Register<RestoreViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
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