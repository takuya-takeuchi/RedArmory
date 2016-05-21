using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RedArmory.Models.Services
{

    [DataContract]
    public sealed class ApplicationSetting : IExtensibleDataObject
    {

        #region プロパティ

        [DataMember]
        public List<RedmineSetting> RedmineSettings
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