using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Extensions
{

    internal static class TriggerExtension
    {

        public static string ToDetailString(this Trigger source)
        {
            var trigger = source as DailyTrigger;
            if (trigger != null)
                return ToDetailStringInternal(trigger);
            var weeklyTrigger = source as WeeklyTrigger;
            if (weeklyTrigger != null)
                return ToDetailStringInternal(weeklyTrigger);
            var monthlyTrigger = source as MonthlyTrigger;
            if (monthlyTrigger != null)
                return ToDetailStringInternal(monthlyTrigger);
            var dowTrigger = source as MonthlyDOWTrigger;
            if (dowTrigger != null)
                return ToDetailStringInternal(dowTrigger);
            return "";
        }

        private static string ToDetailStringInternal(DailyTrigger source)
        {
            var interval = source.DaysInterval;
            var start = source.StartBoundary;

            var str = interval == 1 ? string.Format(Resources.Word_TriggerDailyEveryday, start.ToString("t")) :
                                      string.Format(Resources.Word_TriggerDailyRecurday, interval, start.ToString("t"));

            return str;
        }

        private static string ToDetailStringInternal(WeeklyTrigger source)
        {
            var interval = source.WeeksInterval;
            var start = source.StartBoundary;
            var daysOfTheWeekStr = ToDaysOfTheWeekString(source.DaysOfWeek);

            var str = interval == 1 ? string.Format(Resources.Word_TriggerWeeklyEveryweek, start.ToString("d"), daysOfTheWeekStr, start.ToString("t")) :
                                      string.Format(Resources.Word_TriggerWeeklyRecurweek, start.ToString("d"), daysOfTheWeekStr, start.ToString("t"), interval);

            return str;
        }

        private static string ToDetailStringInternal(MonthlyTrigger source)
        {
            var start = source.StartBoundary;
            var monthOfTheYearStr = ToMonthOfTheYearString(source.MonthsOfYear);

            var daysOfMonthStr = ToDaysOfMonthString(source.DaysOfMonth, source.RunOnLastDayOfMonth);

            var str = string.Format(Resources.Word_TriggerMonthlyDay,
                                    start.ToString("d"),
                                    monthOfTheYearStr,
                                    start.ToString("t"),
                                    daysOfMonthStr);

            return str;
        }

        private static string ToDetailStringInternal(MonthlyDOWTrigger source)
        {
            var start = source.StartBoundary;
            var monthOfTheYearStr = ToMonthOfTheYearString(source.MonthsOfYear);
            var weeksOfMonthStr = ToWeeksOfMonthString(source.WeeksOfMonth, source.RunOnLastWeekOfMonth);
            var daysOfTheWeekStr = ToDaysOfTheWeekString(source.DaysOfWeek);

            var str = string.Format(Resources.Word_TriggerMonthlyWeek,
                                    start.ToString("d"),
                                    daysOfTheWeekStr,
                                    weeksOfMonthStr,
                                    monthOfTheYearStr);

            return str;
        }

        private static string ToDaysOfMonthString(IEnumerable<int> daysOfMonth, bool runOnLastDayOfMonth)
        {
            var daysOfMonthStr = string.Join(", ", daysOfMonth);
            if (runOnLastDayOfMonth)
            {
                daysOfMonthStr += ", " + Resources.Word_Last;
            }

            return daysOfMonthStr;
        }

        private static string ToMonthOfTheYearString(MonthsOfTheYear monthsOfTheYear)
        {
            var dictionary = new[]
            {
                new { Key = MonthsOfTheYear.January,    Resource = Resources.Word_January },
                new { Key = MonthsOfTheYear.February,   Resource = Resources.Word_February },
                new { Key = MonthsOfTheYear.March,      Resource = Resources.Word_March },
                new { Key = MonthsOfTheYear.April,      Resource = Resources.Word_April },
                new { Key = MonthsOfTheYear.May,        Resource = Resources.Word_May },
                new { Key = MonthsOfTheYear.June,       Resource = Resources.Word_June },
                new { Key = MonthsOfTheYear.July,       Resource = Resources.Word_July },
                new { Key = MonthsOfTheYear.August,     Resource = Resources.Word_August },
                new { Key = MonthsOfTheYear.September,  Resource = Resources.Word_September },
                new { Key = MonthsOfTheYear.October,    Resource = Resources.Word_October },
                new { Key = MonthsOfTheYear.November,   Resource = Resources.Word_November },
                new { Key = MonthsOfTheYear.December,   Resource = Resources.Word_December },
            };

            return string.Join(", ", dictionary.Where(arg => (monthsOfTheYear & arg.Key) == arg.Key).Select(arg => arg.Resource));
        }

        private static string ToDaysOfTheWeekString(DaysOfTheWeek daysOfTheWeek)
        {
            var dictionary = new[]
            {
                new { Key = DaysOfTheWeek.Sunday,       Resource = Resources.Word_Sunday },
                new { Key = DaysOfTheWeek.Monday,       Resource = Resources.Word_Monday },
                new { Key = DaysOfTheWeek.Tuesday,      Resource = Resources.Word_Tuesday },
                new { Key = DaysOfTheWeek.Wednesday,    Resource = Resources.Word_Wednesday },
                new { Key = DaysOfTheWeek.Thursday,     Resource = Resources.Word_Thursday },
                new { Key = DaysOfTheWeek.Friday,       Resource = Resources.Word_Friday },
                new { Key = DaysOfTheWeek.Saturday,     Resource = Resources.Word_Saturday },
            };

            return string.Join(", ", dictionary.Where(arg => (daysOfTheWeek & arg.Key) == arg.Key).Select(arg => arg.Resource));
        }

        private static string ToWeeksOfMonthString(WhichWeek whichWeek, bool runOnLastWeekOfMonth)
        {
            var dictionary = new[]
            {
                new { Key = WhichWeek.FirstWeek,    Resource = Resources.Word_First },
                new { Key = WhichWeek.SecondWeek,   Resource = Resources.Word_Second },
                new { Key = WhichWeek.ThirdWeek,    Resource = Resources.Word_Third },
                new { Key = WhichWeek.FourthWeek,   Resource = Resources.Word_Fourth },
            };

            var weeksOfMonthString = string.Join(", ", dictionary.Where(arg => (whichWeek & arg.Key) == arg.Key).Select(arg => arg.Resource));
            if (runOnLastWeekOfMonth)
            {
                weeksOfMonthString += ", " + Resources.Word_Last;
            }

            return weeksOfMonthString;
        }

    }

}
