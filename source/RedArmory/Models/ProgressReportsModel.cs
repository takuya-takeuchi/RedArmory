using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.Models
{

    public sealed class ProgressReportsModel : ViewModelBase
    {

        #region Fields

        private readonly IDispatcherService _DispatcherService;

        #endregion

        #region Constructors

        public ProgressReportsModel(IEnumerable<ProgressItemModel> progressItem)
            :this(null, progressItem)
        {
        }

        public ProgressReportsModel(IDispatcherService dispatcherService, IEnumerable<ProgressItemModel> progressItem)
        {
            this._DispatcherService = dispatcherService;
            this.Progresses = new ObservableCollection<ProgressItemModel>(progressItem);
        }

        #endregion

        #region Properties

        private ObservableCollection<ProgressItemModel> _Progresses;

        public ObservableCollection<ProgressItemModel> Progresses
        {
            get
            {
                return this._Progresses;
            }
            private set
            {
                this._Progresses = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void AddErrorMessage(string name, string message)
        {
            var target = this._Progresses.FirstOrDefault(model => model.Key.Equals(name));

            if (this._DispatcherService != null)
            {
                this._DispatcherService.SafeAction(() => target?.ErrorMessages.Add(message));
            }
            else
            {
                target?.ErrorMessages.Add(message);
            }
        }

        public void UpdateProgress(string name, ProgressState state)
        {
            var target = this._Progresses.FirstOrDefault(model => model.Key.Equals(name));
            if (target != null)
            {
                target.Progress = state;
            }
        }

        #endregion

    }

}