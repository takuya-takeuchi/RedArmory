using System;
using System.Linq;
using System.Windows.Data;
using Microsoft.Win32.TaskScheduler;
using Ouranos.RedArmory.Extensions;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="Task"/> からトリガーを説明する文字列に変換します。このクラスは継承できません。
    /// </summary>
    internal sealed class TaskToTriggerStringConverter : IValueConverter
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
            // トリガーは一つだけという前提
            var trigger = (value as Task)?.Definition?.Triggers?.FirstOrDefault();
            return trigger?.ToDetailString();
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
