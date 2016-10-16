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

        public SettingViewModel(IBitnamiRedmineService bitnamiRedmineService, IRedmineDatabaseConfigurationService databaseConfigurationService, ILoggerService loggerService)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            this._LoggerService = loggerService;
            var bitNamiRedmineStacks = bitnamiRedmineService.GetBitnamiRedmineStacks();

            this.Settings = new ObservableCollection<Setting>(bitNamiRedmineStacks.Select(stack => new Setting(bitnamiRedmineService, databaseConfigurationService, stack)));
            this.IsEmptyStacks = !this.Settings.Any();
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

        private Setting _SelectedSetting;

        public Setting SelectedSetting
        {
            get
            {
                return this._SelectedSetting;
            }
            set
            {
                this._SelectedSetting = value;

                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<Setting> _Settings;

        public ObservableCollection<Setting> Settings
        {
            get
            {
                return this._Settings;
            }
            set
            {
                this._Settings = value;
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