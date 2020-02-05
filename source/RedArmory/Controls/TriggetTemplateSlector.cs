using System.Windows;
using System.Windows.Controls;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory.Controls
{

    internal sealed class TriggetTemplateSlector: DataTemplateSelector
    {

        #region Properties

        public DataTemplate Date
        {
            get;
            set;
        }

        public DataTemplate Month
        {
            get;
            set;
        }

        public DataTemplate OneTime
        {
            get;
            set;
        }

        public DataTemplate Week
        {
            get;
            set;
        }

        #endregion

        #region Methods

        #region Overrides

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="DataTemplate"/> based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>Returns a <see cref="DataTemplate"/> or null. The default value is null.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var oneTime = item as OneTimeTriggerViewModel;
            if (oneTime != null)
            {
                return this.OneTime;
            }

            var date = item as DailyTriggerViewModel;
            if (date != null)
            {
                return this.Date;
            }

            var week = item as WeeklyTriggerViewModel;
            if (week != null)
            {
                return this.Week;
            }

            var month = item as MonthlyTriggerViewModel;
            if (month != null)
            {
                return this.Month;
            }

            return null;
        }

        #endregion

        #endregion

    }

}
