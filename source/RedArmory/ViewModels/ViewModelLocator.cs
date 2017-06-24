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

using System;
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

        private readonly ILogService _LogService;

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

            // ロガーサービス
            SimpleIoc.Default.Register<ILogFactoryService, NLogLogFactoryService>();
            var logFactoryService = SimpleIoc.Default.GetInstance<ILogFactoryService>();

            Register(() => logFactoryService.Create("Main"));
            Register<IDialogService, DialogService>();
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

            this._LogService = SimpleIoc.Default.GetInstance<ILogService>();
        }

        private static void Register<T>(System.Func<T> factory)
            where T : class
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
                SimpleIoc.Default.Register(factory.Invoke);
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
                try
                {
                    return ServiceLocator.Current.GetInstance<AboutViewModel>();
                }
                catch (Exception e)
                {
                    this._LogService.Error(e.Message);
                    throw;
                }
            }
        }

        public BackupViewModel Backups
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<BackupViewModel>();
                }
                catch (Exception e)
                {
                    this._LogService.Error(e.Message);
                    throw;
                }
            }
        }

        public GeneralViewModel General
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<GeneralViewModel>();
                }
                catch (Exception e)
                {
                    this._LogService.Error(e.Message);
                    throw;
                }
            }
        }

        public MainViewModel Main
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<MainViewModel>();
                }
                catch (Exception e)
                {
                    this._LogService.Error(e.Message);
                    throw;
                }
            }
        }
        public RestoreViewModel Restore
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<RestoreViewModel>();
                }
                catch (Exception e)
                {
                    this._LogService.Error(e.Message);
                    throw;
                }
            }
        }

        public SettingViewModel Setting
        {
            get
            {
                try
                {
                    return ServiceLocator.Current.GetInstance<SettingViewModel>();
                }
                catch (Exception e)
                {
                    this._LogService.Error(e.Message);
                    throw;
                }
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}