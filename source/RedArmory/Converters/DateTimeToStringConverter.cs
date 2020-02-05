using System;
using System.Globalization;
using System.Windows.Data;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="DateTime"/>をカルチャに応じた文字列に変換します。このクラスは継承できません。
    /// </summary>
    internal sealed class DateTimeToStringConverter : IValueConverter
    {

        public bool IsSourceUtc
        {
            get;
            set;
        }

        public bool IsOutputUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var datetime = (DateTime)value;

                if (this.IsOutputUtc)
                {
                    if (!this.IsSourceUtc)
                    {
                        datetime = datetime.ToUniversalTime();
                    }
                }
                else
                {
                    if (this.IsSourceUtc)
                    {
                        datetime = datetime.ToLocalTime();
                    }
                }

                if (parameter is DateTimeType)
                {
                    switch ((DateTimeType)parameter)
                    {
                        case DateTimeType.BackupDateTime:
                            return datetime.ToString(Properties.Resources.Format_BackupDate, Properties.Resources.Culture);
                        case DateTimeType.BackupStartTime:
                            return datetime.ToString(Properties.Resources.Format_BackupTaskStartTime, Properties.Resources.Culture);
                        case DateTimeType.BackupStartDate:
                            return datetime.ToString(Properties.Resources.Format_BackupTaskStartDate, Properties.Resources.Culture);
                        case DateTimeType.TaskSchedulerNextRunTime:
                            return datetime != new DateTime() ? datetime.ToString(Properties.Resources.Format_TaskSchedulerDate, Properties.Resources.Culture) : Properties.Resources.Word_TaskSchedulerNextRunTimeEmpty;
                        case DateTimeType.TaskSchedulerLastRunTime:
                            return datetime != new DateTime() ? datetime.ToString(Properties.Resources.Format_TaskSchedulerDate, Properties.Resources.Culture) : Properties.Resources.Word_TaskSchedulerLastRunTimeEmpty;
                    }
                }
                else
                {
                    return datetime.ToString(Properties.Resources.Culture);
                }
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
