﻿using System.Runtime.Serialization;

namespace Ouranos.RedArmory.Models.Services
{

    [DataContract]
    public sealed class BackupSetting : IExtensibleDataObject
    {

        #region Properties

        [DataMember]
        public string BaseDirectory
        {
            get;
            set;
        }

        [DataMember]
        public string DirectoryName
        {
            get;
            set;
        }

        [DataMember]
        public bool Database
        {
            get;
            set;
        }

        [DataMember]
        public bool Files
        {
            get;
            set;
        }

        [DataMember]
        public bool Plugins
        {
            get;
            set;
        }

        [DataMember]
        public bool Themes
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