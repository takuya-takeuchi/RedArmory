using System;
using System.Windows.Data;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="TaskState"/>をカルチャに応じた文字列に変換します。このクラスは継承できません。
    /// </summary>
    internal sealed class TaskStateToStringConverter : IValueConverter
    {

        /// <summary>
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TaskState)
            {
                var state = (TaskState)value;
                switch (state)
                {
                    case TaskState.Unknown:
                        return Properties.Resources.Word_Unknown;
                    case TaskState.Disabled:
                        return Properties.Resources.Word_Disabled;
                    case TaskState.Queued:
                        return Properties.Resources.Word_Queued;
                    case TaskState.Ready:
                        return Properties.Resources.Word_Ready;
                    case TaskState.Running:
                        return Properties.Resources.Word_Running;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
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
