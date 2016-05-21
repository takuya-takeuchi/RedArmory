using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RedArmory.Models.Services
{

    [DataContract]
    public sealed class ApplicationSetting : IExtensibleDataObject
    {

        #region コンストラクタ
        public ApplicationSetting()
        {
            this.BackupHistories = new List<BackupHistorySetting>();
            this.RedmineSettings = new List<RedmineSetting>();
        }

        #endregion

        #region プロパティ

        [DataMember]
        public List<BackupHistorySetting> BackupHistories
        {
            get;
            set;
        }

        [DataMember]
        public List<RedmineSetting> RedmineSettings
        {
            get;
            set;
        }

        #endregion

        #region メソッド

        [OnDeserializing]
        public void OnDeserializing(StreamingContext sc)
        {
            this.BackupHistories = new List<BackupHistorySetting>();
            this.RedmineSettings = new List<RedmineSetting>();
        }

        #endregion

        #region IExtensibleDataObject メンバー

        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        #endregion

    }

}