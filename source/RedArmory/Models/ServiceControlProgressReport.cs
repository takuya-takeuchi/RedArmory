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
        /// Apache に対する作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>Apache に対する作業の進捗状況である <see cref="ProgressState"/>。</returns>
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
        /// Redmine に対する作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>Redmine に対する作業の進捗状況である <see cref="ProgressState"/>。</returns>
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
        /// MySql に対する作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>MySql に対する作業の進捗状況である <see cref="ProgressState"/>。</returns>
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
        /// Subversion に対する作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>Subversion に対する作業の進捗状況である <see cref="ProgressState"/>。</returns>
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