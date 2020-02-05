using System;
using System.Windows.Data;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="ServiceStartupType"/> から <see cref="string"/> への変換を行うコンバーターを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class ServiceStartupTypeToStringConverter : IValueConverter
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
            var status = (ServiceStartupType)value;
            switch (status)
            {
                case ServiceStartupType.Disabled:
                    return Properties.Resources.Word_ServiceDisabled;
                case ServiceStartupType.Manual:
                    return Properties.Resources.Word_ServiceManual;
                case ServiceStartupType.Automatic:
                    return Properties.Resources.Word_ServiceAutomatic;
                case ServiceStartupType.DelayStart:
                    return Properties.Resources.Word_ServiceDelayedStart;
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
            throw new NotImplementedException();
        }
    }

}
