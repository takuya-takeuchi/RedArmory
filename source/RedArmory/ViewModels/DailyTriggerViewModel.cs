using System;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class DailyTriggerViewModel : TriggerViewModel
    {

        #region プロパティ

        private ushort _Interval = 1;

        public ushort Interval
        {
            get
            {
                return this._Interval;
            }
            set
            {
                this._Interval = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        public override Trigger GetTrigger()
        {
            var dailyTrigger = new DailyTrigger((short)this.Interval);
            var d = this.Date;
            var t = this.Time;
            dailyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

            return dailyTrigger;
        }

        #endregion

    }

}