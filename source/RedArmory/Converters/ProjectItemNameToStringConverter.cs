using System;
using System.Windows.Data;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="ProjectItem.Name"/> を 適切な表現への変換を行うコンバーターを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class ProjectItemNameToStringConverter : IValueConverter
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
            var item = value as ProjectItem;
            if (item == null)
            {
                return null;
            }

            return item.Id == 0 ? Properties.Resources.Word_ProjectAll : item.Name;
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
            throw new NotImplementedException();
        }
    }

}
