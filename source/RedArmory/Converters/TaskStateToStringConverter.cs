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
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
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
