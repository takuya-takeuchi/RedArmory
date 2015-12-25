namespace RedArmory.Models.Services
{

    public sealed class BitnamiRedmineStack
    {

        #region �R���X�g���N�^

        public BitnamiRedmineStack(string installLocation, string displayVersion)
        {
            this.InstallLocation = installLocation;
            this.DisplayVersion = displayVersion;
        }

        #endregion

        #region �v���p�e�B

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