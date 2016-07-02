using System.Collections.ObjectModel;

namespace Ouranos.RedArmory.Models.Services
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