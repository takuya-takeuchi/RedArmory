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
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.GetVisibility(value);
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
            throw new NotImplementedException();
        }

        #region ヘルパーメソッド

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
