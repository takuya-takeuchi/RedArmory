using System;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models.DatabaseObjects;

namespace Ouranos.RedArmory.Models
{

    public sealed class EnumerationItem : ViewModelBase
    {

        #region コンストラクタ

        internal EnumerationItem(EnumerationObject item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            this.Id = item.Id;
            this.IsActive = item.Active == 1;
            this.IsDefault = item.Is_Default == 1;
            this.Name = item.Name;
            this.ParentId = item.Parent_Id;
            this.Position = item.Position;
            this.PositionName = item.Position_Name;
            this.ProjectId = item.Project_Id;
            this.Type = item.Type;
        }

        #endregion

        #region プロパティ

        private int _Id;

        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _IsDefault;

        public bool IsDefault
        {
            get
            {
                return this._IsDefault;
            }
            set
            {
                this._IsDefault = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _IsLast;

        public bool IsLast
        {
            get
            {
                return this._IsLast;
            }
            set
            {
                this._IsLast = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _IsTop;

        public bool IsTop
        {
            get
            {
                return this._IsTop;
            }
            set
            {
                this._IsTop = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _IsActive;

        public bool IsActive
        {
            get
            {
                return this._IsActive;
            }
            set
            {
                this._IsActive = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Type;

        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
                this.RaisePropertyChanged();
            }
        }

        private int _Position;

        public int Position
        {
            get
            {
                return this._Position;
            }
            set
            {
                this._Position = value;
                this.RaisePropertyChanged();
            }
        }

        private int? _ParentId;

        public int? ParentId
        {
            get
            {
                return this._ParentId;
            }
            set
            {
                this._ParentId = value;
                this.RaisePropertyChanged();
            }
        }

        private string _PositionName;

        public string PositionName
        {
            get
            {
                return this._PositionName;
            }
            set
            {
                this._PositionName = value;
                this.RaisePropertyChanged();
            }
        }

        private int? _ProjectId;

        public int? ProjectId
        {
            get
            {
                return this._ProjectId;
            }
            set
            {
                this._ProjectId = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}