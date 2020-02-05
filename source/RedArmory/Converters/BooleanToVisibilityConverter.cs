using System;
using System.Windows;
using System.Windows.Data;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// ブール値から Visibility 列挙値への変換や、その逆の変換を行うコンバーターを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class BooleanToVisibilityConverter : IValueConverter
    {

        private bool _IsHidden;

        public bool IsHidden
        {
            get
            {
                return this._IsHidden;
            }
            set
            {
                this._IsHidden = value;
            }
        }

        private bool _TriggerValue;

        public bool TriggerValue
        {
            get
            {
                return this._TriggerValue;
            }
            set
            {
                this._TriggerValue = value;
            }
        }

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
            return this.GetVisibility(value);
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

        #region Helpers

        private object GetVisibility(object value)
        {
            if (!(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            var objValue = (bool)value;
            if ((objValue && this.TriggerValue && this.IsHidden) || (!objValue && !this.TriggerValue && this.IsHidden))
            {
                return Visibility.Hidden;
            }

            if ((objValue && this.TriggerValue && !this.IsHidden) || (!objValue && !this.TriggerValue && !this.IsHidden))
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        #endregion

    }

}
