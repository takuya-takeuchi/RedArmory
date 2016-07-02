using System;
using System.Runtime.Serialization;

namespace Ouranos.RedArmory.Models.Services
{

    [DataContract]
    public sealed class BackupHistorySetting : IExtensibleDataObject
    {

        #region プロパティ

        [DataMember]
        public string DisplayVersion
        {
            get;
            set;
        }

        [DataMember]
        public DateTime DateTime
        {
            get;
            set;
        }

        [DataMember]
        public string OutputDirectory
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