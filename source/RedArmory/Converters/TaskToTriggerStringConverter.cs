using System;
using System.Linq;
using System.Windows.Data;
using Microsoft.Win32.TaskScheduler;
using Ouranos.RedArmory.Extensions;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="Task"/> からトリガーを説明する文字列に変換します。このクラスは継承できません。
    /// </summary>
    internal sealed class TaskToTriggerStringConverter : IValueConverter
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
            // トリガーは一つだけという前提
            var trigger = (value as Task)?.Definition?.Triggers?.FirstOrDefault();
            return trigger?.ToDetailString();
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
