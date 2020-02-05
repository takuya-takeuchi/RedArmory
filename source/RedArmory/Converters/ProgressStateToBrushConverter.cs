using System;
using System.Windows.Data;
using System.Windows.Media;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="ProgressState"/>を<see cref="Brush"/>に変換します。このクラスは継承できません。
    /// </summary>
    internal sealed class ProgressStateToBrushConverter : IValueConverter
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
                        return Brushes.Black;
                    case ProgressState.InProgress:
                        return Brushes.YellowGreen;
                    case ProgressState.Complete:
                        return Brushes.Green;
                    case ProgressState.NotRequire:
                        return Brushes.Gray;
                    case ProgressState.Failed:
                        return Brushes.Red;
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
