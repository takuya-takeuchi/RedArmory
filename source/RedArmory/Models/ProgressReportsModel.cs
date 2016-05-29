﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using RedArmory.Models.Services;

namespace RedArmory.Models
{

    public sealed class ProgressReportsModel : ViewModelBase
    {

        #region フィールド

        private readonly IDispatcherService _DispatcherService;

        #endregion

        #region コンストラクタ

        public ProgressReportsModel(IEnumerable<ProgressItemModel> progressItem)
        {
            this._DispatcherService = null;
            this.Progresses = new ObservableCollection<ProgressItemModel>(progressItem);
        }

        public ProgressReportsModel(IDispatcherService dispatcherService, IEnumerable<ProgressItemModel> progressItem)
        {
            if (dispatcherService == null)
                throw new ArgumentNullException(nameof(dispatcherService));

            this._DispatcherService = dispatcherService;
            this.Progresses = new ObservableCollection<ProgressItemModel>(progressItem);
        }

        #endregion

        #region プロパティ

        private ObservableCollection<ProgressItemModel> _Progresses;

        public ObservableCollection<ProgressItemModel> Progresses
        {
            get
            {
                return this._Progresses;
            }
            private set
            {
                this._Progresses = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        public void AddErrorMessage(string name, string message)
        {
            var target = this._Progresses.FirstOrDefault(model => model.Key.Equals(name));
            this._DispatcherService.SafeAction(() => target?.ErrorMessages.Add(message));
        }

        public void UpdateProgress(string name, ProgressState state)
        {
            var target = this._Progresses.FirstOrDefault(model => model.Key.Equals(name));
            if (target != null)
            {
                target.Progress = state;
            }
        }

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }

}