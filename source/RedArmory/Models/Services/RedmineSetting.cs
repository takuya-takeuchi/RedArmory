using System.Runtime.Serialization;

namespace Ouranos.RedArmory.Models.Services
{
    
    [DataContract]
    public sealed class RedmineSetting : IExtensibleDataObject
    {

        #region Constructors

        public RedmineSetting()
        {
            this.Backup = new BackupSetting();
            this.Restore = new RestoreSetting();
        }

        #endregion

        #region Properties

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

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        #endregion

    }

}