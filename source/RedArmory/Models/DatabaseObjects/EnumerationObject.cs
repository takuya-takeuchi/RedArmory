using System;

namespace Ouranos.RedArmory.Models.DatabaseObjects
{

    /// <summary>
    /// bitnami_redmine.enumerations に対応するオブジェクトを表します。
    /// </summary>
    internal sealed class EnumerationObject
    {

        #region コンストラクタ

        internal EnumerationObject()
        {
            // 消さないこと   
        }

        internal EnumerationObject(EnumerationItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            this.Id = item.Id;
            this.Is_Default = item.IsDefault ? 1 : 0;
            this.Active = item.IsActive ? 1 : 0;
            this.Position = item.Position;
            this.Name = item.Name;
            this.Parent_Id = item.ParentId;
            this.Position_Name = item.PositionName;
            this.Project_Id = item.ProjectId;
            this.Type = item.Type;
        }

        #endregion

        #region プロパティ

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public int Is_Default
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public int Active
        {
            get;
            set;
        }

        public int? Project_Id
        {
            get;
            set;
        }

        public int? Parent_Id
        {
            get;
            set;
        }

        public string Position_Name
        {
            get;
            set;
        }

        #endregion

    }

}