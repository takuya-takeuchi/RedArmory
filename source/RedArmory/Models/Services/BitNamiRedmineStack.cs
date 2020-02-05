namespace Ouranos.RedArmory.Models.Services
{

    public sealed class BitnamiRedmineStack
    {

        #region Constructors

        public BitnamiRedmineStack(string installLocation, string displayVersion)
        {
            this.InstallLocation = installLocation;
            this.DisplayVersion = displayVersion;
        }

        #endregion

        #region Properties

        public string DisplayVersion
        {
            get;
            private set;
        }

        public string InstallLocation
        {
            get;
            private set;
        }

        #endregion

    }

}