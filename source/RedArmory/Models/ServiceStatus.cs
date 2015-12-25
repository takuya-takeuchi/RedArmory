using GalaSoft.MvvmLight;
using RedArmory.Models.Services;

namespace RedArmory.Models
{

    public sealed class ServiceStatus : ViewModelBase
    {

        public ServiceStatus(string serviceName, ServiceStartupType startupType)
        {
            this._ServiceName = serviceName;
            this._StartupType = startupType;
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
                    BitnamiRedmineService.Instance.SetStartupType(this._ServiceName, value);
                }
            }
        }

    }

}