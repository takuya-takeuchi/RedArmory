using System;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class OneTimeTriggerViewModel : TriggerViewModel
    {

        #region メソッド

        public override Trigger GetTrigger()
        {
            var dailyTrigger = new DailyTrigger(0);
            var d = this.Date;
            var t = this.Time;
            dailyTrigger.StartBoundary = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

            return dailyTrigger;
        }

        #region オーバーライド

        protected override bool ValidateCondition()
        {
            return true;
        }

        #endregion

        #endregion

    }

}