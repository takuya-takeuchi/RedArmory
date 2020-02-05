using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class MonthlyTriggerViewModel : TriggerViewModel
    {

        #region Fields

        private const int RunOnLastDayOfMonthIndex = 31;

        private const int RunOnLastWeekOfMonthIndex = 4;

        private const int DaysCount = 32;

        private const int WeekCount = 5;

        private const int MonthCount = 12;

        #endregion

        #region Constructors

        public MonthlyTriggerViewModel()
        {
            this._Days = Enumerable.Repeat(0, DaysCount).Select(i => new SingleValueWapperModel<bool>()).ToArray(); // 1 - 31, �ŏI
            this._Weeks = Enumerable.Repeat(0, WeekCount).Select(i => new SingleValueWapperModel<bool>()).ToArray(); // 1 - 4, �ŏI
            this._Months = Enumerable.Repeat(0, MonthCount).Select(i => new SingleValueWapperModel<bool>()).ToArray();

            foreach (var model in this._Days)
                model.PropertyChanged += this.OnPropertyChanged;
            foreach (var model in this._Weeks)
                model.PropertyChanged += this.OnPropertyChanged;
            foreach (var model in this._Months)
                model.PropertyChanged += this.OnPropertyChanged;
        }

        #endregion

        #region Properties

        private SingleValueWapperModel<bool>[] _Days;

        public SingleValueWapperModel<bool>[] Days
        {
            get
            {
                return this._Days;
            }
            set
            {
                this._Days = value;
                this.RaisePropertyChanged();
            }
        }

        private SingleValueWapperModel<bool>[] _Months;

        public SingleValueWapperModel<bool>[] Months
        {
            get
            {
                return this._Months;
            }
            set
            {
                this._Months = value;
                this.RaisePropertyChanged();
            }
        }

        private SingleValueWapperModel<bool>[] _Weeks;

        public SingleValueWapperModel<bool>[] Weeks
        {
            get
            {
                return this._Weeks;
            }
            set
            {
                this._Weeks = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _UseDay = true;

        public bool UseDay
        {
            get
            {
                return this._UseDay;
            }
            set
            {
                this._UseDay = value;
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
            var monthsOfTheYear = this.GetMonthsOfTheYear();

            if (this.UseDay)
            {
                var monthlyTrigger = new MonthlyTrigger();
                monthlyTrigger.MonthsOfYear = monthsOfTheYear;
                monthlyTrigger.DaysOfMonth = this.GetDaysOfMonth();
                monthlyTrigger.RunOnLastDayOfMonth = this.Days[RunOnLastDayOfMonthIndex].Value;

                var d = this.Date;
                var t = this.Time;
                monthlyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

                return monthlyTrigger;
            }
            else
            {
                var daysOfTheWeek = this.GetDaysOfTheWeek();
                var whichWeek = this.GetWhichWeek();

                var monthlyTrigger = new MonthlyDOWTrigger();
                monthlyTrigger.MonthsOfYear = monthsOfTheYear;
                monthlyTrigger.DaysOfWeek = daysOfTheWeek;
                monthlyTrigger.WeeksOfMonth = whichWeek;
                monthlyTrigger.RunOnLastWeekOfMonth = this.Weeks[RunOnLastWeekOfMonthIndex].Value;

                var d = this.Date;
                var t = this.Time;
                monthlyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

                return monthlyTrigger;
            }
        }

        #region Overrides

        protected override bool ValidateCondition()
        {
            var monthsOfTheYear = this.GetMonthsOfTheYear();
            if (monthsOfTheYear == 0)
            {
                return false;
            }

            if (!this.UseDay)
            {
                var daysOfTheWeek = this.GetDaysOfTheWeek();
                if (daysOfTheWeek == 0)
                {
                    return false;
                }

                var whichWeek = this.GetWhichWeek();
                if (whichWeek == 0 && !this.Weeks[RunOnLastWeekOfMonthIndex].Value)
                {
                    return false;
                }
            }
            else
            {
                var daysOfMonth = this.GetDaysOfMonth();
                if (!daysOfMonth.Any() && !this.Days[RunOnLastDayOfMonthIndex].Value)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Event Handlers

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.RaisePropertyChanged();
        }

        #endregion

        #region Helpers

        private int[] GetDaysOfMonth()
        {
            return this.Days.Select((model, i) => model.Value && (i != RunOnLastDayOfMonthIndex) ? i + 1 : 0).Where(i => i != 0).ToArray();
        }

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

        private MonthsOfTheYear GetMonthsOfTheYear()
        {
            if (this.Months == null)
            {
                return 0;
            }

            var monthsOfTheYear = MonthsOfTheYear.AllMonths;
            if (!this.Months[0].Value)
                monthsOfTheYear ^= MonthsOfTheYear.January;
            if (!this.Months[1].Value)
                monthsOfTheYear ^= MonthsOfTheYear.February;
            if (!this.Months[2].Value)
                monthsOfTheYear ^= MonthsOfTheYear.March;
            if (!this.Months[3].Value)
                monthsOfTheYear ^= MonthsOfTheYear.April;
            if (!this.Months[4].Value)
                monthsOfTheYear ^= MonthsOfTheYear.May;
            if (!this.Months[5].Value)
                monthsOfTheYear ^= MonthsOfTheYear.June;
            if (!this.Months[6].Value)
                monthsOfTheYear ^= MonthsOfTheYear.July;
            if (!this.Months[7].Value)
                monthsOfTheYear ^= MonthsOfTheYear.August;
            if (!this.Months[8].Value)
                monthsOfTheYear ^= MonthsOfTheYear.September;
            if (!this.Months[9].Value)
                monthsOfTheYear ^= MonthsOfTheYear.October;
            if (!this.Months[10].Value)
                monthsOfTheYear ^= MonthsOfTheYear.November;
            if (!this.Months[11].Value)
                monthsOfTheYear ^= MonthsOfTheYear.December;

            return monthsOfTheYear;
        }

        private WhichWeek GetWhichWeek()
        {
            if (this.Weeks == null)
            {
                return 0;
            }

            var whichWeek = WhichWeek.AllWeeks;
            if (!this.Weeks[0].Value)
                whichWeek ^= WhichWeek.FirstWeek;
            if (!this.Weeks[1].Value)
                whichWeek ^= WhichWeek.SecondWeek;
            if (!this.Weeks[2].Value)
                whichWeek ^= WhichWeek.ThirdWeek;
            if (!this.Weeks[3].Value)
                whichWeek ^= WhichWeek.FourthWeek;

            return whichWeek;
        }

        #endregion

        #endregion

    }

}