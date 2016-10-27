using System;
using System.Windows.Data;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory.Converters
{

    /// <summary>
    /// <see cref="TriggerViewModel"/> から <see cref="string"/> への変換を行うコンバーターを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class TriggerViewModelToStringConverter : IValueConverter
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
            var viewModel = value as TriggerViewModel;
            if (viewModel == null)
            {
                return null;
            }

            if (viewModel is OneTimeTriggerViewModel)
            {
                return Properties.Resources.Word_TriggerOneTime;
            }
            else if (viewModel is DailyTriggerViewModel)
            {
                return Properties.Resources.Word_TriggerDaily;
            }
            else if (viewModel is WeeklyTriggerViewModel)
            {
                return Properties.Resources.Word_TriggerWeekly;
            }
            else if (viewModel is MonthlyTriggerViewModel)
            {
                return Properties.Resources.Word_TriggerMonthly;
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
            throw new NotImplementedException();
        }
    }

}
