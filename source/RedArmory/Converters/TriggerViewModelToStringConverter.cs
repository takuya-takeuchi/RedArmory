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
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
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
