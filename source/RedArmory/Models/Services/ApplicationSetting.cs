using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ouranos.RedArmory.Models.Services
{

    [DataContract]
    public sealed class ApplicationSetting : IExtensibleDataObject
    {

        #region Constructors
        public ApplicationSetting()
        {
            this.BackupHistories = new List<BackupHistorySetting>();
            this.RedmineSettings = new List<RedmineSetting>();
        }

        #endregion

        #region Properties

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

        #region Methods

        [OnDeserializing]
        public void OnDeserializing(StreamingContext sc)
        {
            this.BackupHistories = new List<BackupHistorySetting>();
            this.RedmineSettings = new List<RedmineSetting>();
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