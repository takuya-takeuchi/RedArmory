using System.Runtime.Serialization;

namespace Ouranos.RedArmory.Models.Services
{

    [DataContract]
    public sealed class RestoreSetting : IExtensibleDataObject
    {

        #region Properties

        [DataMember]
        public string Directory
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