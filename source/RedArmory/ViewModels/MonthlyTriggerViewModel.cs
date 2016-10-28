using System;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class MonthlyTriggerViewModel : TriggerViewModel
    {

        #region コンストラクタ

        public MonthlyTriggerViewModel()
        {
            this._Days = Enumerable.Repeat(0, 32).Select(i => new SingleValueWapperModel<bool>()).ToArray(); // 1 - 31, 最終
            this._Weeks = Enumerable.Repeat(0, 5).Select(i => new SingleValueWapperModel<bool>()).ToArray(); // 1 - 4, 最終
            this._Months = Enumerable.Repeat(0, 12).Select(i => new SingleValueWapperModel<bool>()).ToArray();
        }

        #endregion

        #region プロパティ

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

        #region メソッド

        public override Trigger GetTrigger()
        {
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

            if (this.UseDay)
            {
                var monthlyTrigger = new MonthlyTrigger();
                monthlyTrigger.MonthsOfYear = monthsOfTheYear;
                monthlyTrigger.RunOnLastDayOfMonth = this.Days[31].Value;

                var d = this.Date;
                var t = this.Time;
                monthlyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

                return monthlyTrigger;
            }
            else
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

                var whichWeek = WhichWeek.AllWeeks;
                if (!this.Weeks[0].Value)
                    whichWeek ^= WhichWeek.FirstWeek;
                if (!this.Weeks[1].Value)
                    whichWeek ^= WhichWeek.SecondWeek;
                if (!this.Weeks[2].Value)
                    whichWeek ^= WhichWeek.ThirdWeek;
                if (!this.Weeks[3].Value)
                    whichWeek ^= WhichWeek.FourthWeek;

                var monthlyTrigger = new MonthlyDOWTrigger();
                monthlyTrigger.MonthsOfYear = monthsOfTheYear;
                monthlyTrigger.DaysOfWeek = daysOfTheWeek;
                monthlyTrigger.WeeksOfMonth = whichWeek;
                monthlyTrigger.RunOnLastWeekOfMonth = this.Weeks[4].Value;

                var d = this.Date;
                var t = this.Time;
                monthlyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

                return monthlyTrigger;
            }
        }

        #endregion

    }

}