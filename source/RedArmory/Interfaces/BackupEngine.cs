using System;
using System.Collections.Generic;
using System.Linq;
using Ouranos.RedArmory.Extensions;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Interfaces
{

    public sealed class BackupEngine
    {

        #region Fields

        private readonly BackupConfiguration _Configuration;

        private readonly IBitnamiRedmineService _BitnamiRedmineService;

        private readonly IBackupService _BackupService;

        private readonly IDispatcherService _DispatcherService;

        private readonly ILogService _LogService;

        private readonly BitnamiRedmineStack _Stack;

        private readonly string _OutputPath;

        private IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> _ApacheServices;

        private IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> _MySqlServices;

        private IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> _SubversionServices;

        private IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> _RedmineServices;

        private KeyValuePair<ServiceStatus, ProgressItemModel>[] _BeforeServiceReports;

        private IEnumerable<ProgressItemModel> _MainTaskReports;

        private IEnumerable<ProgressItemModel> _AfterServiceReports;

        #endregion

        #region Constructors

        public BackupEngine(
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            IDispatcherService dispatcherService,
            ILogService logService,
            BackupConfiguration configuration,
            BitnamiRedmineStack stack,
            string outputPath)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (stack == null)
                throw new ArgumentNullException(nameof(stack));
            
            this._BitnamiRedmineService = bitnamiRedmineService;
            this._BackupService = backupService;
            this._DispatcherService = dispatcherService;
            this._LogService = logService;
            this._Configuration = configuration;
            this._Stack = stack;
            this._OutputPath = outputPath;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        public ProgressReportsModel PrepareBackup()
        {
            var bitnamiRedmineService = this._BitnamiRedmineService;
            var stack = this._Stack;
            var configuration = this._Configuration;
            var dispatcherService = this._DispatcherService;

            // Apache
            this._ApacheServices = bitnamiRedmineService.GetServiceDisplayNames(stack, new ServiceConfiguration
            {
                Apache = true,
                MySql = false,
                Redmine = false,
                Subversion = false
            }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(false)));

            // MySql
            this._MySqlServices = bitnamiRedmineService.GetServiceDisplayNames(stack, new ServiceConfiguration
            {
                Apache = false,
                MySql = true,
                Redmine = false,
                Subversion = false
            }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(configuration.Database)));

            // Subversion
            this._SubversionServices = bitnamiRedmineService.GetServiceDisplayNames(stack, new ServiceConfiguration
            {
                Apache = false,
                MySql = false,
                Redmine = false,
                Subversion = true
            }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(false)));

            // Redmine
            this._RedmineServices = bitnamiRedmineService.GetServiceDisplayNames(stack, new ServiceConfiguration
            {
                Apache = false,
                MySql = false,
                Redmine = true,
                Subversion = false
            }).Select(status => new KeyValuePair<ServiceStatus, ProgressItemModel>(status, status.ToProgressItemModel(false)));

            this._BeforeServiceReports =
                this._ApacheServices.Concat(
                this._MySqlServices).Concat(
                this._SubversionServices).Concat(
                this._RedmineServices).ToArray();

            this._MainTaskReports = new[]
            {
                new ProgressItemModel {Key = Resources.Word_Database, TaskName = Resources.Word_Database,Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = Resources.Word_Plugin, TaskName = Resources.Word_Plugin,Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = Resources.Word_Theme, TaskName = Resources.Word_Theme,Progress = ProgressState.NotStart},
                new ProgressItemModel {Key = Resources.Word_AttachedFile, TaskName = Resources.Word_AttachedFile,Progress = ProgressState.NotStart},
            };

            this._AfterServiceReports = bitnamiRedmineService.GetServiceDisplayNames(stack, new ServiceConfiguration
            {
                Apache = true,
                MySql = true,
                Redmine = true,
                Subversion = true
            }).Select(status => status.ToProgressItemModel(true)).ToArray();

            return new ProgressReportsModel(dispatcherService, 
                this._BeforeServiceReports.Select(pair => pair.Value).Concat(this._MainTaskReports).Concat(this._AfterServiceReports));
        }

        public void ExecuteBackup()
        {
            this.ExecuteBackup(
                this._ApacheServices,
                this._MySqlServices,
                this._SubversionServices,
                this._RedmineServices,
                this._BeforeServiceReports,
                this._MainTaskReports,
                this._AfterServiceReports);
        }

        #region Helpers

        private void ExecuteBackup(
            IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> apacheServices,
            IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> mySqlServices,
            IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> subversionServices,
            IEnumerable<KeyValuePair<ServiceStatus, ProgressItemModel>> redmineServices,
            KeyValuePair<ServiceStatus, ProgressItemModel>[] beforeServiceReports,
            IEnumerable<ProgressItemModel> mainTaskReports,
            IEnumerable<ProgressItemModel> afterServiceReports)
        {
            var backupService = this._BackupService;
            var bitnamiRedmineService = this._BitnamiRedmineService;
            var stack = this._Stack;
            var configuration = this._Configuration;
            var path = this._OutputPath;
            var dispatcherService = this._DispatcherService;

            // サービスの停止
            var beforeTargets = new[]
            {
                new { Reports = apacheServices,       RequiredStart = false },
                new { Reports = mySqlServices,        RequiredStart = configuration.Database },
                new { Reports = subversionServices,   RequiredStart = false },
                new { Reports = redmineServices,      RequiredStart = false },
            };

            var beforeServiceProgress = new Progress<ProgressReportsModel>(progressReport =>
            {
                foreach (var p in progressReport.Progresses)
                {
                    var progressItemModel = beforeServiceReports.Select(kvp => kvp.Value).FirstOrDefault(model => model.Key.Equals(p.Key));
                    if (progressItemModel == null)
                        continue;

                    progressItemModel.Progress = p.Progress;
                    progressItemModel.ErrorMessages = p.ErrorMessages;
                }
            });

            foreach (var t in beforeTargets)
                foreach (var s in t.Reports)
                    if (t.RequiredStart)
                        bitnamiRedmineService.StartService(s.Key, new ProgressReportsModel(dispatcherService, new[] { s.Value }), beforeServiceProgress);
                    else
                        bitnamiRedmineService.StopService(s.Key, new ProgressReportsModel(dispatcherService, new[] { s.Value }), beforeServiceProgress);


            // バックアップ処理
            backupService.Backup(stack, configuration, path, new Progress<ProgressReportsModel>(
                progressReport =>
                {
                    foreach (var p in progressReport.Progresses)
                    {
                        var progressItemModel = mainTaskReports.FirstOrDefault(model => model.Key.Equals(p.Key));
                        if (progressItemModel == null)
                            continue;

                        progressItemModel.Progress = p.Progress;
                        progressItemModel.ErrorMessages = p.ErrorMessages;
                    }
                }));

            // サービスの開始
            var afterTargets = new[]
            {
                new { Reports = apacheServices,       RequiredStart = true },
                new { Reports = mySqlServices,        RequiredStart = true },
                new { Reports = subversionServices,   RequiredStart = true },
                new { Reports = redmineServices,      RequiredStart = true },
            };

            var afterServiceProgress = new Progress<ProgressReportsModel>(progressReport =>
            {
                foreach (var p in progressReport.Progresses)
                {
                    var progressItemModel = afterServiceReports.FirstOrDefault(model => model.Key.Equals(p.Key));
                    if (progressItemModel == null)
                        continue;

                    progressItemModel.Progress = p.Progress;
                    progressItemModel.ErrorMessages = p.ErrorMessages;
                }
            });

            foreach (var t in afterTargets)
                foreach (var s in t.Reports)
                    if (t.RequiredStart)
                        bitnamiRedmineService.StartService(s.Key, new ProgressReportsModel(dispatcherService, new[] { s.Value }), afterServiceProgress);
                    else
                        bitnamiRedmineService.StopService(s.Key, new ProgressReportsModel(dispatcherService, new[] { s.Value }), afterServiceProgress);
        }

        #endregion

        #endregion

    }

}
