namespace Ouranos.RedArmory.Models
{

    /// <summary>
    /// ディスクの特定のカテゴリのサイズを表します。このクラスは継承できません。
    /// </summary>
    internal sealed class DiskInfo
    {

        public string Category
        {
            get; set;
        }

        public long Number
        {
            get; set;
        }

    }

}