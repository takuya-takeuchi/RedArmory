using System;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models.DatabaseObjects;

namespace Ouranos.RedArmory.Models
{

    public sealed class ProjectItem : ViewModelBase
    {

        #region Constructors

        internal ProjectItem(ProjectObject item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            this.Id = item.Id;
            this.Name = item.Name;
            this.Description = item.Description;
            this.Homepage = item.Homepage;
            this.IsPublic = item.Is_Public;
            this.ParentId = item.Parent_Id;
            this.CreatedOn = item.Created_On;
            this.UpdatedOn = item.Updated_On;
            this.Identifier = item.Identifier;
            this.Status = item.Status;
            this.Lft = item.Lft;
            this.Rgt = item.Rgt;
            this.InheritMembers = item.Inherit_Members;
            this.DefaultVersionId = item.Default_Version_Id;
        }

        #endregion

        #region Properties


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

        private string _Description;

        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Homepage;

        public string Homepage
        {
            get
            {
                return this._Homepage;
            }
            set
            {
                this._Homepage = value;
                this.RaisePropertyChanged();
            }
        }

        private int _IsPublic;

        public int IsPublic
        {
            get
            {
                return this._IsPublic;
            }
            set
            {
                this._IsPublic = value;
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

        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get
            {
                return this._CreatedOn;
            }
            set
            {
                this._CreatedOn = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime _UpdatedOn;

        public DateTime UpdatedOn
        {
            get
            {
                return this._UpdatedOn;
            }
            set
            {
                this._UpdatedOn = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Identifier;

        public string Identifier
        {
            get
            {
                return this._Identifier;
            }
            set
            {
                this._Identifier = value;
                this.RaisePropertyChanged();
            }
        }

        private int _Status;

        public int Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this._Status = value;
                this.RaisePropertyChanged();
            }
        }

        private int _Lft;

        public int Lft
        {
            get
            {
                return this._Lft;
            }
            set
            {
                this._Lft = value;
                this.RaisePropertyChanged();
            }
        }

        private int _Rgt;

        public int Rgt
        {
            get
            {
                return this._Rgt;
            }
            set
            {
                this._Rgt = value;
                this.RaisePropertyChanged();
            }
        }

        private int? _InheritMembers;

        public int? InheritMembers
        {
            get
            {
                return this._InheritMembers;
            }
            set
            {
                this._InheritMembers = value;
                this.RaisePropertyChanged();
            }
        }

        private int? _DefaultVersionId;

        public int? DefaultVersionId
        {
            get
            {
                return this._DefaultVersionId;
            }
            set
            {
                this._DefaultVersionId = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}