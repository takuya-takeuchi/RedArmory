using System;
using System.Windows.Data;
using System.Windows.Media;
using Ouranos.RedArmory.Models;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="ProgressState"/>を<see cref="Brush"/>に変換します。このクラスは継承できません。
    /// </summary>
    public sealed class ProgressStateToBrushConverter : IValueConverter
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
        /// 値を変換します。
        /// </summary>
        /// <param name="value">バインディング ターゲットによって生成される値。</param>
        /// <param name="targetType">変換後の型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャ。</param>
        /// <returns>変換された値。 メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
