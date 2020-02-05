using System;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class WeeklyTriggerViewModel : TriggerViewModel
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

        private bool _Sunday;

        public bool Sunday
        {
            get
            {
                return this._Sunday;
            }
            set
            {
                this._Sunday = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _Monday;

        public bool Monday
        {
            get
            {
                return this._Monday;
            }
            set
            {
                this._Monday = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _Tuesday;

        public bool Tuesday
        {
            get
            {
                return this._Tuesday;
            }
            set
            {
                this._Tuesday = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _Wednesday;

        public bool Wednesday
        {
            get
            {
                return this._Wednesday;
            }
            set
            {
                this._Wednesday = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _Thursday;

        public bool Thursday
        {
            get
            {
                return this._Thursday;
            }
            set
            {
                this._Thursday = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _Friday;

        public bool Friday
        {
            get
            {
                return this._Friday;
            }
            set
            {
                this._Friday = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _Saturday;

        public bool Saturday
        {
            get
            {
                return this._Saturday;
            }
            set
            {
                this._Saturday = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public override Trigger GetTrigger()
        {
            var daysOfTheWeek = this.GetDaysOfTheWeek();
            var weeklyTrigger = new WeeklyTrigger();
            weeklyTrigger.WeeksInterval = (short)this.Interval;
            weeklyTrigger.DaysOfWeek = daysOfTheWeek;

            var d = this.Date;
            var t = this.Time;
            weeklyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

            return weeklyTrigger;
        }

        #region Overrides

        protected override bool ValidateCondition()
        {
            if(this._Interval <= 0 )
            {
                return false;
            }

            var daysOfTheWeek = this.GetDaysOfTheWeek();
            if (daysOfTheWeek == 0)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Helpers

        private DaysOfTheWeek GetDaysOfTheWeek()
        {
            var daysOfTheWeek = DaysOfTheWeek.AllDays;
            if (!this.Sunday)
                daysOfTheWeek ^= DaysOfTheWeek.Sunday;
            if (!this.Monday)
                daysOfTheWeek ^= DaysOfTheWeek.Monday;
            if (!this.Tuesday)
                daysOfTheWeek ^= DaysOfTheWeek.Tuesday;
            if (!this.Wednesday)
                daysOfTheWeek ^= DaysOfTheWeek.Wednesday;
            if (!this.Thursday)
                daysOfTheWeek ^= DaysOfTheWeek.Thursday;
            if (!this.Friday)
                daysOfTheWeek ^= DaysOfTheWeek.Friday;
            if (!this.Saturday)
                daysOfTheWeek ^= DaysOfTheWeek.Saturday;

            return daysOfTheWeek;
        }

        #endregion

        #endregion

    }

}