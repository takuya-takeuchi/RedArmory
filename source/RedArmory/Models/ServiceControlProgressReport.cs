using GalaSoft.MvvmLight;

namespace Ouranos.RedArmory.Models
{
    
    internal sealed class ServiceControlProgressReport : ViewModelBase
    {

        #region コンストラクタ

        public ServiceControlProgressReport()
        {
            this.Apache = ProgressState.NotStart;
            this.Redmine = ProgressState.NotStart;
            this.MySql = ProgressState.NotStart;
            this.Subversion = ProgressState.NotStart;
        }

        #endregion

        #region プロパティ

        private ProgressState _Apache;

        /// <summary>
        /// Apache �̑���̐i���󋵂�擾�܂��͐ݒ肵�܂��B
        /// </summary>
        /// <returns>Apache �̑���̐i���� <see cref="ProgressState"/>�B</returns>
        public ProgressState Apache
        {
            get
            {
                return this._Apache;
            }
            set
            {
                this._Apache = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _Redmine;

        /// <summary>
        /// Redmine �̑���̐i���󋵂�擾�܂��͐ݒ肵�܂��B
        /// </summary>
        /// <returns>Redmine �̑���̐i���� <see cref="ProgressState"/>�B</returns>
        public ProgressState Redmine
        {
            get
            {
                return this._Redmine;
            }
            set
            {
                this._Redmine = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _MySql;

        /// <summary>
        /// MySql �̑���̐i���󋵂�擾�܂��͐ݒ肵�܂��B
        /// </summary>
        /// <returns>MySql �̑���̐i���� <see cref="ProgressState"/>�B</returns>
        public ProgressState MySql
        {
            get
            {
                return this._MySql;
            }
            set
            {
                this._MySql = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _Subversion;

        /// <summary>
        /// Subversion �̑���̐i���󋵂�擾�܂��͐ݒ肵�܂��B
        /// </summary>
        /// <returns>Subversion �̑���̐i���� <see cref="ProgressState"/>�B</returns>
        public ProgressState Subversion
        {
            get
            {
                return this._Subversion;
            }
            set
            {
                this._Subversion = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}