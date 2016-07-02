using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace Ouranos.RedArmory.ViewModels
{

    public sealed class SettingViewModel : ViewModelBase
    {

        #region フィールド

        private readonly ILoggerService _LoggerService;

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

        public SettingViewModel(IBitnamiRedmineService bitnamiRedmineService, ILoggerService loggerService)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            this._LoggerService = loggerService;
            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Stacks = new ObservableCollection<Setting>(bitNamiRedmineStacks.Select(stack => new Setting(bitnamiRedmineService, stack)));
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