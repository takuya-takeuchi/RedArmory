using System;
using System.Windows.Data;
using RedArmory.Models;

namespace RedArmory.Converters
{

    /// <summary>
    /// <see cref="ProgressState"/>をカルチャに応じた文字列に変換します。このクラスは継承できません。
    /// </summary>
    public sealed class ProgressStateToStringConverter : IValueConverter
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
