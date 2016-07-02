namespace Ouranos.RedArmory.Models.Services
{

    public sealed class BackupConfiguration
    {

        #region フィールド

        public static readonly string FilesPath = @"apps\redmine\htdocs\files";

        public static readonly string PluginsPath = @"apps\redmine\htdocs\plugins";

        public static readonly string ThemesePath = @"apps\redmine\htdocs\public\themes";

        #endregion

        #region コンストラクタ

        public BackupConfiguration()
        {
        }

        #endregion

        #region プロパティ

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