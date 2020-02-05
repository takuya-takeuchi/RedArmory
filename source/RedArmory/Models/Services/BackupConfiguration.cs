namespace Ouranos.RedArmory.Models.Services
{

    public sealed class BackupConfiguration
    {

        #region Fields

        public static readonly string FilesPath = @"apps\redmine\htdocs\files";

        public static readonly string PluginsPath = @"apps\redmine\htdocs\plugins";

        public static readonly string ThemesePath = @"apps\redmine\htdocs\public\themes";

        #endregion

        #region Constructors

        public BackupConfiguration()
        {
        }

        #endregion

        #region Properties

        public bool Database
        {
            get;
            set;
        }

        public bool Files
        {
            get;
            set;
        }

        public bool Plugins
        {
            get;
            set;
        }

        public bool Themes
        {
            get;
            set;
        }

        #endregion

    }

}