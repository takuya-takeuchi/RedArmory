namespace RedArmory.Models.Services
{

    public sealed class BitNamiRedmineStack
    {

        #region �R���X�g���N�^

        public BitNamiRedmineStack(string installLocation, string displayVersion)
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