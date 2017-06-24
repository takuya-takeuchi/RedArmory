using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models.DatabaseObjects;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory.Models
{

    internal sealed class Setting : ViewModelBase
    {

        #region イベント
        #endregion

        #region フィールド

        private readonly IDatabaseConnectorService _DatabaseConnectorService;

        private readonly IDialogService _DialogService;
        private readonly ILogService _LogService;

        private readonly DatabaseConfiguration _DatabaseConfiguration;

        private readonly BitnamiRedmineStack _Stack;

        #endregion

        #region コンストラクタ

        static Setting()
        {
            _EnumerationTypes = new[]
            {
                EnumerationType.DocumentCategory,
                EnumerationType.IssuePriority,
                EnumerationType.TimeEntryActivity
            };
        }

        public Setting(IBitnamiRedmineService bitnamiRedmineService, IRedmineDatabaseConfigurationService databaseConfigurationService, ITaskService taskService, IDialogService dialogService, ILogService logService, BitnamiRedmineStack stack)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (databaseConfigurationService == null)
                throw new ArgumentNullException(nameof(databaseConfigurationService));

            if (taskService == null)
                throw new ArgumentNullException(nameof(taskService));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            this._Stack = stack;
            this._DialogService = dialogService;
            this._LogService = logService;

            var configuration = new ServiceConfiguration
            {
                Apache = true,
                MySql = true,
                Redmine = true,
                Subversion = true
            };

            var serviceStatuses = bitnamiRedmineService.GetServiceDisplayNames(stack, configuration);
            this._ServiceStatuses = new ObservableCollection<ServiceStatus>(serviceStatuses);

            this._DatabaseConfiguration = databaseConfigurationService.GetDatabaseConfiguration(stack).FirstOrDefault();
            this._DatabaseConnectorService = new MySqlConnectorService(this._DatabaseConfiguration, logService);

            var projects = this._DatabaseConnectorService.GetProjects().ToList();
            projects.Insert(0, new ProjectItem(new ProjectObject { Id = 0, Name = "[全体]" }));
            this._Projects = new ObservableCollection<ProjectItem>(projects);

            this._SelectedProject = this._Projects.First();
            this._SelectedEnumerationType = EnumerationTypes.First();

            this.UpdateSelectedEnumeration();
            this.SelectedTaskScheduler = new TaskSchedulerViewModel(stack, taskService, dialogService);
        }

        #endregion

        #region プロパティ

        private static IEnumerable<EnumerationType> _EnumerationTypes;

        public static IEnumerable<EnumerationType> EnumerationTypes
        {
            get
            {
                return _EnumerationTypes;
            }
        }

        private ObservableCollection<ProjectItem> _Projects;

        public ObservableCollection<ProjectItem> Projects
        {
            get
            {
                return this._Projects;
            }
        }

        private EnumerationViewModel _SelectedEnumeration;

        public EnumerationViewModel SelectedEnumeration
        {
            get
            {
                return this._SelectedEnumeration;
            }
            set
            {
                this._SelectedEnumeration = value;
                value?.RefreshCommand.Execute(null);
                this.RaisePropertyChanged();
            }
        }

        private EnumerationType _SelectedEnumerationType;

        public EnumerationType SelectedEnumerationType
        {
            get
            {
                return this._SelectedEnumerationType;
            }
            set
            {
                this._SelectedEnumerationType = value;
                this.RaisePropertyChanged();

                this.UpdateSelectedEnumeration();
            }
        }

        private ProjectItem _SelectedProject;

        public ProjectItem SelectedProject
        {
            get
            {
                return this._SelectedProject;
            }
            set
            {
                this._SelectedProject = value;
                this.RaisePropertyChanged();

                this.UpdateSelectedEnumeration();
            }
        }

        private TaskSchedulerViewModel _SelectedTaskScheduler;

        public TaskSchedulerViewModel SelectedTaskScheduler
        {
            get
            {
                return this._SelectedTaskScheduler;
            }
            set
            {
                this._SelectedTaskScheduler = value;
                value?.RefreshCommand.Execute(null);
                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<ServiceStatus> _ServiceStatuses;

        public ObservableCollection<ServiceStatus> ServiceStatuses
        {
            get
            {
                return this._ServiceStatuses;
            }
        }

        public BitnamiRedmineStack Stack
        {
            get
            {
                return this._Stack;
            }
        }

        #endregion

        #region メソッド

        #region オーバーライド

        #endregion

        #region イベントハンドラ

        #endregion

        #region ヘルパーメソッド

        private void UpdateSelectedEnumeration()
        {
            this.SelectedEnumeration = new EnumerationViewModel(this._DatabaseConfiguration,
                                                                this._SelectedProject,
                                                                this._SelectedEnumerationType, 
                                                                this._DialogService,
                                                                this._LogService);
        }

        #endregion

        #endregion

    }

}