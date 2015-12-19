namespace RedArmory.Models.Services
{

    public sealed class BitNamiRedmineStack
    {

        #region コンストラクタ

        public BitNamiRedmineStack(string installLocation, string displayVersion)
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