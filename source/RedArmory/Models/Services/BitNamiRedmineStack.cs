namespace RedArmory.Models.Services
{

    public sealed class BitnamiRedmineStack
    {

        #region コンストラクタ

        public BitnamiRedmineStack(string installLocation, string displayVersion)
        {
            this.InstallLocation = installLocation;
            this.DisplayVersion = displayVersion;
        }

        #endregion

        #region プロパティ

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