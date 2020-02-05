namespace Ouranos.RedArmory.Models
{

    internal sealed class LanguageModel
    {

        #region Properties

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