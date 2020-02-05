using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.ViewModels
{

    internal sealed class TaskViewModel : ViewModelBase
    {

        #region Constructors

        public TaskViewModel()
        {
            this._Triggers = new TriggerViewModel[]
            {
                //new OneTimeTriggerViewModel(), // One time のトリガーの作成方法が不明なため除去
                new DailyTriggerViewModel(),
                new WeeklyTriggerViewModel(),
                new MonthlyTriggerViewModel()
            };
            this._SelectedTrigger = this._Triggers.FirstOrDefault();
        }

        #endregion

        #region Properties

        private string _Description;

        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }

        private TriggerViewModel _SelectedTrigger;

        public TriggerViewModel SelectedTrigger
        {
            get
            {
                return this._SelectedTrigger;
            }
            set
            {
                this._SelectedTrigger = value;
                this.RaisePropertyChanged();
            }
        }

        private IEnumerable<TriggerViewModel> _Triggers;

        public IEnumerable<TriggerViewModel> Triggers
        {
            get
            {
                return this._Triggers;
            }
        }

        #endregion

    }

}