using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using RedArmory.Models;

namespace RedArmory.ViewModels
{

    public sealed class GeneralViewModel : ViewModelBase
    {

        #region コンストラクタ

        public GeneralViewModel()
        {
            var languageModels = new[]
            {
                new LanguageModel 
                {
                    Name = "en-US",
                    DisplayName = Properties.Resources.Lang_English
                },
                new LanguageModel
                {
                    Name = "ja-JP",
                    DisplayName = Properties.Resources.Lang_Japanese
                },
            };

            this.Languages = new ObservableCollection<LanguageModel>(languageModels);

            var culture = CultureInfo.CurrentCulture;
            var languageModel = this.Languages.FirstOrDefault(model => model.Name.Equals(culture.Name));
            this.SelectedLanguage = languageModel ?? languageModels[0];
        }

        #endregion

        #region プロパティ

        private ObservableCollection<LanguageModel> _Languages;

        public ObservableCollection<LanguageModel> Languages
        {
            get
            {
                return this._Languages;
            }
            set
            {
                this._Languages = value;
                this.RaisePropertyChanged();
            }
        }

        private LanguageModel _SelectedLanguage;

        public LanguageModel SelectedLanguage
        {
            get
            {
                return this._SelectedLanguage;
            }
            set
            {
                this._SelectedLanguage = value;

                this.RaisePropertyChanged();

                ResourceService.Instance.ChangeCulture(value.Name);
            }
        }

        #endregion

    }

}
