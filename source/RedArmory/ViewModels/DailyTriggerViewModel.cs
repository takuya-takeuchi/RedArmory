using System;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class DailyTriggerViewModel : TriggerViewModel
    {

        #region Properties

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

        #region Methods

        public override Trigger GetTrigger()
        {
            var dailyTrigger = new DailyTrigger((short)this.Interval);
            var d = this.Date;
            var t = this.Time;
            dailyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

            return dailyTrigger;
        }

        #region Overrides

        protected override bool ValidateCondition()
        {
            return this._Interval > 0;
        }

        #endregion

        #endregion

    }

}