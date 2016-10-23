using System;
using GalaSoft.MvvmLight;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.ViewModels
{

    internal abstract class TriggerViewModel : ViewModelBase
    {

        #region コンストラクタ

        protected TriggerViewModel()
        {
            this.Date = DateTime.Now;
            this.Time = this.Date;
        }

        #endregion

        #region プロパティ

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

        #endregion

    }

}
