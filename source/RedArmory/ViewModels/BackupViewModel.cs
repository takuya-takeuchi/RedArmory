﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class BackupViewModel : BitnamiStackCommonViewModel<BackupModel>
    {

        #region Fields

        private readonly ITaskService _TaskService;

        #endregion

        #region Constructors

        public BackupViewModel(
            IApplicationSettingService applicationSettingService,
            IBitnamiRedmineService bitnamiRedmineService,
            IBackupService backupService,
            ITaskService taskService,
            IDispatcherService dispatcherService,
            IDialogService dialogService,
            ILogService logService)
            : base(dialogService, logService)
        {
            if (applicationSettingService == null)
                throw new ArgumentNullException(nameof(applicationSettingService));

            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (backupService == null)
                throw new ArgumentNullException(nameof(backupService));

            if (taskService == null)
                throw new ArgumentNullException(nameof(taskService));

            if (dispatcherService == null)
                throw new ArgumentNullException(nameof(dispatcherService));

            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<BackupModel>(bitNamiRedmineStacks.Select(
                stack => new BackupModel(applicationSettingService, bitnamiRedmineService, backupService, dispatcherService, dialogService, logService, stack)));
            
            this._TaskService = taskService;
            this.CreateTaskCommand = new RelayCommand(this.ExecuteCreateTask, this.CanCreateTaskExecute);
        }

        #endregion

        #region Properties

        public RelayCommand CreateTaskCommand
        {
            get;
            private set;
        }

        private readonly TaskViewModel _Task = new TaskViewModel();

        public TaskViewModel Task
        {
            get
            {
                return this._Task;
            }
        }

        #endregion

        #region Methods

        #region Helpers

        private bool CanCreateTaskExecute()
        {
            return true;
        }

        private string CreateArguments(BackupModel backupModel)
        {
            var args = new List<string>();

            // 動作モード
            args.Add("-m");
            args.Add("backup");

            // バックアップ先ディレクトリ
            args.Add("-o");
            args.Add($"\"{backupModel.Directory.TrimEnd('\\')}\"");

            // バージョン
            args.Add("-v");
            args.Add(backupModel.Stack.DisplayVersion);

            // バックアップ先ディレクトリの名前
            args.Add("-n");
            args.Add($"\"{backupModel.DirectoryName}\"");

            // テーマをバックアップまたは復元の対象にするかどうか
            if (backupModel.Themes)
            {
                args.Add("-t");
            }

            // データベースをバックアップまたは復元の対象にするかどうか
            if (backupModel.Database)
            {
                args.Add("-d");
            }

            // プラグインをバックアップまたは復元の対象にするかどうか
            if (backupModel.Plugins)
            {
                args.Add("-p");
            }

            // 添付ファイルをバックアップまたは復元の対象にするかどうか
            if (backupModel.Files)
            {
                args.Add("-a");
            }

            return string.Join(" ", args);
        }

        private async void ExecuteCreateTask()
        {
            var stack = this.SelectedStack;
            var task = this.Task;

            if (this._TaskService.IsExist(task.Name))
            {
                var result = await this._DialogService.ShowMessage(MessageBoxButton.YesNo, Resources.Msg_TaskOverwrite, null);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            string message;

            try
            {
                this._TaskService.Create(new TaskSetting
                {
                    Name = task.Name,
                    Description = task.Description,
                    Argument = this.CreateArguments(stack),
                    Trigger = task.SelectedTrigger.GetTrigger()
                });

                message = Resources.Msg_TaskCreateSuccess;
            }
            catch (Exception ex)
            {
                this._LogService.Error($"Failed to create task. Exception is {ex}");
                message = Resources.Msg_TaskCreateFailed;
            }
            
            await this._DialogService.ShowMessage(MessageBoxButton.OK, message, null);
        }

        #endregion

        #endregion

    }

}