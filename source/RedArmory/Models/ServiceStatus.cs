using System;
using GalaSoft.MvvmLight;
using RedArmory.Models.Services;

namespace RedArmory.Models
{

    public sealed class ServiceStatus : ViewModelBase
    {

        #region フィールド

        private readonly IBitnamiRedmineService _BitnamiRedmineService;

        #endregion

        #region コンストラクタ

        public ServiceStatus(IBitnamiRedmineService bitnamiRedmineService, string serviceName, ServiceStartupType startupType)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            this._BitnamiRedmineService = bitnamiRedmineService;
            this._ServiceName = serviceName;
            this._StartupType = startupType;
        }

        #endregion

        #region プロパティ

        private bool _StopRequire;

        public bool StopRequire
        {
            get
            {
                return this._StopRequire;
            }
            set
            {
                this._StopRequire = value;
                this.RaisePropertyChanged();
            }
        }

        private string _ServiceName;

        public string ServiceName
        {
            get
            {
                return this._ServiceName;
            }
            set
            {
                this._ServiceName = value;
                this.RaisePropertyChanged();
            }
        }

        private ServiceStartupType _StartupType;

        public ServiceStartupType StartupType
        {
            get
            {
                return this._StartupType;
            }
            set
            {
                var needUpdate = this._StartupType != value;

                this._StartupType = value;
                this.RaisePropertyChanged();

                if (needUpdate)
                {
                    this._BitnamiRedmineService.SetStartupType(this._ServiceName, value);
                }
            }
        }

        #endregion

    }

}