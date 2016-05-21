using System.Runtime.Serialization;

namespace RedArmory.Models.Services
{

    [DataContract]
    public sealed class RestoreSetting : IExtensibleDataObject
    {

        #region プロパティ

        [DataMember]
        public string Directory
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