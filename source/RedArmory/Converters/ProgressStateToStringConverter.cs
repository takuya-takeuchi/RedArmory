using System;
using System.Windows.Data;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="ProgressState"/>をカルチャに応じた文字列に変換します。このクラスは継承できません。
    /// </summary>
    internal sealed class ProgressStateToStringConverter : IValueConverter
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
            if (value is ProgressState)
            {
                var state = (ProgressState)value;
                switch (state)
                {
                    case ProgressState.NotStart:
                        return Properties.Resources.Word_ProgressState_NotStart;
                    case ProgressState.InProgress:
                        return Properties.Resources.Word_ProgressState_InProgress;
                    case ProgressState.Complete:
                        return Properties.Resources.Word_ProgressState_Complete;
                    case ProgressState.NotRequire:
                        return Properties.Resources.Word_ProgressState_NotRequire;
                    case ProgressState.Failed:
                        return Properties.Resources.Word_ProgressState_Failed;
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
