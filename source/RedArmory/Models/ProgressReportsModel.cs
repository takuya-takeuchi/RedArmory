using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace RedArmory.Models
{
    
    public sealed class ProgressReportsModel : ViewModelBase
    {

        #region コンストラクタ

        public ProgressReportsModel(IEnumerable<ProgressItemModel> progressItem)
        {
            this.Progresses = new ObservableCollection<ProgressItemModel>(progressItem);
        }

        #endregion

        #region プロパティ

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

        #region メソッド

        public void UpdateProgress(string name, ProgressState state)
        {
            var target = this._Progresses.FirstOrDefault(model => model.Name.Equals(name));
            if (target != null)
            {
                target.Progress = state;
            }
        }

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }

}