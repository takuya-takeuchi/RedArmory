using System.Collections.ObjectModel;

namespace RedArmory.Models.Services
{

    public interface IApplicationSettingService
    {

        ObservableCollection<BackupHistorySetting> BackupHistories
        {
            get;
        }

        ApplicationSetting GetApplicationSetting();

        void UpdateApplicationSetting(ApplicationSetting applicationSetting);

    }

}