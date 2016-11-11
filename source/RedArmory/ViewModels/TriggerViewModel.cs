using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.ViewModels
{

    internal abstract class TriggerViewModel : ViewModelBase
    {

        #region コンストラクタ

        protected TriggerViewModel()
        {
            this.PropertyChanged += this.OnPropertyChanged;
            this.Date = DateTime.Now;
            this.Time = this.Date;
        }

        #endregion

        #region プロパティ

        private bool _CanCreateTask;

        public bool CanCreateTask
        {
            get
            {
                return this._CanCreateTask;
            }
            private set
            {
                this._CanCreateTask = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                this._Date = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime _Time;

        public DateTime Time
        {
            get
            {
                return this._Time;
            }
            set
            {
                this._Time = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        public abstract Trigger GetTrigger();

        protected abstract bool ValidateCondition();

        #region イベントハンドラ

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(this.CanCreateTask))
                this.CanCreateTask = this.ValidateCondition();
        }

        #endregion

        #endregion

    }

}
