using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.ViewModels
{

    public sealed class SettingViewModel : ViewModelBase
    {

        #region フィールド

        public static ServiceStartupType[] ServiceStartupTypes;

        #endregion

        #region コンストラクタ

        static SettingViewModel()
        {
            ServiceStartupTypes = new[]
            {
                ServiceStartupType.Automatic, 
                ServiceStartupType.DelayStart, 
                ServiceStartupType.Manual, 
                ServiceStartupType.Disabled
            };
        }

        public SettingViewModel()
        {
            var bitNamiRedmineStacks = BitnamiRedmineService.Instance.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<Setting>(bitNamiRedmineStacks.Select(stack => new Setting(stack)));
            this.IsEmptyStacks = !this.Stacks.Any();
        }

        #endregion

        #region プロパティ

        private bool _IsEmptyStacks;

        public bool IsEmptyStacks
        {
            get
            {
                return this._IsEmptyStacks;
            }
            set
            {
                this._IsEmptyStacks = value;
                this.RaisePropertyChanged();
            }
        }

        public bool HasStacks
        {
            get
            {
                return !this._IsEmptyStacks;
            }
            set
            {
                this.RaisePropertyChanged();
            }
        }

        private Setting _SelectedStack;

        public Setting SelectedStack
        {
            get
            {
                return this._SelectedStack;
            }
            set
            {
                this._SelectedStack = value;

                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<Setting> _Stacks;

        public ObservableCollection<Setting> Stacks
        {
            get
            {
                return this._Stacks;
            }
            set
            {
                this._Stacks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region メソッド

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド

        #endregion

        #endregion

    }

}