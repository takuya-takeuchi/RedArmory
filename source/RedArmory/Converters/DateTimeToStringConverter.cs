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
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
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
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。 メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
