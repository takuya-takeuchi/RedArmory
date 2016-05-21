using System.Runtime.Serialization;

namespace RedArmory.Models.Services
{
    
    [DataContract]
    public sealed class RedmineSetting : IExtensibleDataObject
    {

        #region コンストラクタ

        public RedmineSetting()
        {
            this.Backup = new BackupSetting();
            this.Restore = new RestoreSetting();
        }

        #endregion

        #region プロパティ

        [DataMember]
        public string DisplayVersion
        {
            get;
            set;
        }

        [DataMember]
        public BackupSetting Backup
        {
            get;
            set;
        }

        [DataMember]
        public RestoreSetting Restore
        {
            get;
            set;
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