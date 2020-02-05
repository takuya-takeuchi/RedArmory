using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Ouranos.RedArmory.ViewModels;

namespace Ouranos.RedArmory.Controls
{
    /// <summary>
    /// DateTimeControl.xaml の相互作用ロジック
    /// </summary>
    internal partial class DateTimeControl : UserControl
    {

        #region Constructors

        public DateTimeControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public TriggerViewModel ViewModel
        {
            get
            {
                return this.DataContext as TriggerViewModel;
            }
        }

        #endregion

        #region Methods

        #region Event Handlers

        public void CalendarDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            var trigger = this.ViewModel;
            if (trigger == null)
            {
                return;
            }

            this._Calendar.SelectedDate = trigger.Date;
        }

        public void CalendarDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            var trigger = this.ViewModel;
            if (trigger == null)
            {
                return;
            }

            if (!Equals(eventArgs.Parameter, "1"))
            {
                return;
            }

            if (!this._Calendar.SelectedDate.HasValue)
            {
                eventArgs.Cancel();
                return;
            }

            trigger.Date = this._Calendar.SelectedDate.Value;
        }

        public void ClockDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            var trigger = this.ViewModel;
            if (trigger == null)
            {
                return;
            }

            this._Clock.Time = trigger.Time;
        }

        public void ClockDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            var trigger = this.ViewModel;
            if (trigger == null)
            {
                return;
            }

            if (Equals(eventArgs.Parameter, "1"))
            {
                trigger.Time = this._Clock.Time;
            }
        }

        #endregion

        #endregion

    }

}
