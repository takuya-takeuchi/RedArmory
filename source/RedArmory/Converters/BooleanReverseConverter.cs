using System;
using System.Windows.Data;

namespace RedArmory.Converters
{

    /// <summary>
    /// ブール値の逆転を行うコンバーターを表します。このクラスは継承できません。
    /// </summary>
    public sealed class BooleanReverseConverter : IValueConverter
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
            if (value is bool)
            {
                return !(bool) value;
            }

            throw new ArgumentException();
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
            if (value is bool)
            {
                return !(bool)value;
            }

            throw new ArgumentException();
        }

    }

}
