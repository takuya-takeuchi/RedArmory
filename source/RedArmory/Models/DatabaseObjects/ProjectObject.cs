using System;

namespace Ouranos.RedArmory.Models.DatabaseObjects
{

    /// <summary>
    /// bitnami_redmine.projects �ɑΉ�����I�u�W�F�N�g��\���܂��B
    /// </summary>
    internal sealed class ProjectObject
    {

        #region �R���X�g���N�^

        internal ProjectObject()
        {
            // �����Ȃ�����
        }

        internal ProjectObject(EnumerationItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
        }

        #endregion

        #region �v���p�e�B


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

        public string Description
        {
            get;
            set;
        }

        public string Homepage
        {
            get;
            set;
        }

        public int Is_Public
        {
            get;
            set;
        }

        public int? Parent_Id
        {
            get;
            set;
        }

        public DateTime Created_On
        {
            get;
            set;
        }

        public DateTime Updated_On
        {
            get;
            set;
        }

        public string Identifier
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public int Lft
        {
            get;
            set;
        }

        public int Rgt
        {
            get;
            set;
        }

        public int? Inherit_Members
        {
            get;
            set;
        }

        public int? Default_Version_Id
        {
            get;
            set;
        }

        #endregion

    }

}