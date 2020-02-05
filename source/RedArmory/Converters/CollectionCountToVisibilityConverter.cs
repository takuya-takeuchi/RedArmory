using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// コレクションの要素数が指定した要素数以下の時、要素の Visibility 列挙値を指定した Visibility へ変換するコンバーターを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class CollectionCountToVisibilityConverter : IValueConverter
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

        private int _TriggerCount;

        public int TriggerCount
        {
            get
            {
                return this._TriggerCount;
            }
            set
            {
                this._TriggerCount = value;
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
            var collection = value as ICollection;
            if (collection == null)
            {
                return Visibility.Collapsed;
            }

            if (collection.Count <= this._TriggerCount)
            {
                return this.IsHidden ? Visibility.Hidden : Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        #endregion

    }

}
