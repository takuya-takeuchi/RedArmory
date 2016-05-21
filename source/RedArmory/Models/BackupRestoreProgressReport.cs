using GalaSoft.MvvmLight;

namespace RedArmory.Models
{

    /// <summary>
    /// バックアップ、復元作業の進捗状況を表します。このクラスは継承できません。
    /// </summary>
    public sealed class BackupRestoreProgressReport : ViewModelBase
    {

        #region コンストラクタ

        public BackupRestoreProgressReport()
        {
            this.Database = ProgressState.NotStart;
            this.Plugin = ProgressState.NotStart;
            this.AttachedFile = ProgressState.NotStart;
            this.Theme = ProgressState.NotStart;
        }

        #endregion

        #region プロパティ

        private ProgressState _Database;

        /// <summary>
        /// データベースのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>データベースのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState Database
        {
            get
            {
                return this._Database;
            }
            set
            {
                this._Database = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _Plugin;

        /// <summary>
        /// プラグインのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>プラグインのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState Plugin
        {
            get
            {
                return this._Plugin;
            }
            set
            {
                this._Plugin = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _AttachedFile;

        /// <summary>
        /// 添付ファイルのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>添付ファイルのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState AttachedFile
        {
            get
            {
                return this._AttachedFile;
            }
            set
            {
                this._AttachedFile = value;
                this.RaisePropertyChanged();
            }
        }

        private ProgressState _Theme;

        /// <summary>
        /// テーマのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>テーマのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState Theme
        {
            get
            {
                return this._Theme;
            }
            set
            {
                this._Theme = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}