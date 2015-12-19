namespace RedArmory.Models
{

    /// <summary>
    /// バックアップ、復元作業の進捗状況を表します。このクラスは継承できません。
    /// </summary>
    public sealed class BackupRestoreProgressReport
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

        /// <summary>
        /// データベースのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>データベースのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState Database
        {
            get;
            set;
        }

        /// <summary>
        /// プラグインのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>プラグインのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState Plugin
        {
            get;
            set;
        }

        /// <summary>
        /// 添付ファイルのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>添付ファイルのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState AttachedFile
        {
            get;
            set;
        }

        /// <summary>
        /// テーマのバックアップ、復元の作業の進捗状況を取得または設定します。
        /// </summary>
        /// <returns>テーマのバックアップ、復元の作業の進捗状況 <see cref="ProgressState"/>。</returns>
        public ProgressState Theme
        {
            get;
            set;
        }

        #endregion

    }

}