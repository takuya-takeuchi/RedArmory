namespace RedArmory.Models
{

    /// <summary>
    /// 作業の進捗状況を示します。
    /// </summary>
    public enum ProgressState
    {

        /// <summary>
        /// 作業が開始されていないことを示します。
        /// </summary>
        NotStart,

        /// <summary>
        /// 作業が進捗中であることを示します。
        /// </summary>
        InProgress,

        /// <summary>
        /// 作業が完了したことを示します。
        /// </summary>
        Complete,

        /// <summary>
        /// 作業が必要ないことを示します。
        /// </summary>
        NotRequire,

        /// <summary>
        /// 作業が失敗したことを示します。
        /// </summary>
        Failed

    }

}