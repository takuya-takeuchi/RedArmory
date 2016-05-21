namespace RedArmory.Models.Services
{

    public sealed class ServiceConfiguration
    {

        #region プロパティ

        public bool Apache
        {
            get;
            set;
        }

        public bool MySql
        {
            get;
            set;
        }

        public bool Redmine
        {
            get;
            set;
        }

        public bool Subversion
        {
            get;
            set;
        }

        #endregion

    }

}