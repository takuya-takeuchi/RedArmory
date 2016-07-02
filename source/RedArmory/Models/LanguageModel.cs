namespace Ouranos.RedArmory.Models
{
    public sealed class LanguageModel
    {

        #region プロパティ

        private string _DisplayName;

        public string DisplayName
        {
            get
            {
                return this._DisplayName;
            }
            set
            {
                this._DisplayName = value;
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
            }
        }

        #endregion

    }

}