using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RedArmory.Models.Services;

namespace RedArmory.Models
{

    public class Setting : ViewModelBase
    {

        #region イベント
        #endregion

        #region フィールド

        private readonly BitNamiRedmineStack _Stack;

        #endregion

        #region コンストラクタ

        public Setting(BitNamiRedmineStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("stack");
            }

            this._Stack = stack;

            var configuration = new ServiceConfiguration
            {
                Apache = true,
                MySql = true,
                Redmine = true,
                Subversion = true
            };

            var serviceStatuses = BitNamiRedmineService.Instance.GetServiceDisplayNames(stack, configuration);
            this.ServiceStatuses = new ObservableCollection<ServiceStatus>(serviceStatuses);
        }

        #endregion

        #region プロパティ

        private ObservableCollection<ServiceStatus> _ServiceStatuses;

        public ObservableCollection<ServiceStatus> ServiceStatuses
        {
            get
            {
                return this._ServiceStatuses;
            }
            set
            {
                this._ServiceStatuses = value;
                this.RaisePropertyChanged();
            }
        }

        public BitNamiRedmineStack Stack
        {
            get
            {
                return this._Stack;
            }
        }

        #endregion

        #region メソッド

        #region オーバーライド

        #endregion

        #region イベントハンドラ

        #endregion

        #region ヘルパーメソッド

        #endregion

        #endregion
    }
}