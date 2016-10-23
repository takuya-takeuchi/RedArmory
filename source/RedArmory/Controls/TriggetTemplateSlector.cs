using System.Windows;
using System.Windows.Controls;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory.Controls
{

    internal sealed class TriggetTemplateSlector: DataTemplateSelector
    {

        #region プロパティ

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

        #region メソッド

        #region オーバーライド

        /// <summary>
        /// 派生クラスでオーバーライドされると、カスタム ロジックに基づいて DataTemplate を返します。
        /// </summary>
        /// <param name="item">テンプレートを選択する対象のデータ オブジェクト。</param>
        /// <param name="container">データ バインドされたオブジェクト。</param>
        /// <returns><see cref="DataTemplate"/> または null を返します。既定値は null です。</returns>
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
