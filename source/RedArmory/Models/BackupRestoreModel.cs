using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedArmory.Models.Services;

namespace RedArmory.Models
{

    public abstract class BackupRestoreModel : ViewModelBase
    {

        #region フィールド

        private readonly BitNamiRedmineStack _Stack;

        #endregion

        #region コンストラクタ

        protected BackupRestoreModel(BitNamiRedmineStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("stack");
            }

            this._Stack = stack;

            this.Database = true;
            this.Files = true;
            this.Plugins = true;
            this.Themes = true;

            this.Command = new RelayCommand(this.Execute, this.CanExecute);
            this.SelectDirectoryCommand = new RelayCommand(this.ExecuteSelectDirectory, this.CanExecuteSelectDirectory);


#if DEBUG
            var dest = @"f:\RedArmony\Test";
            System.IO.Directory.CreateDirectory(dest);
            this.Directory = dest;
#else
            
            this.UpdateDiskSpace();
#endif
            //var bitNamiRedmineStackConfiguration = ConfigurationService.Instance.GetBitNamiRedmineStackConfiguration(stack.DisplayVersion);
            //bitNamiRedmineStackConfiguration.
        }

        #endregion

        #region プロパティ

        public RelayCommand Command
        {
            get;
            protected set;
        }

        public RelayCommand SelectDirectoryCommand
        {
            get;
            protected set;
        }

        private string _Directory;

        public string Directory
        {
            get
            {
                return this._Directory;
            }
            set
            {
                this._Directory = value;
                this.RaisePropertyChanged();
            }
        }

        public BitNamiRedmineStack Stack
        {
            get
            {
                return this._Stack;
            }
        }

        private bool _Database;

        public bool Database
        {
            get
            {
                return this._Database;
            }
            set
            {
                this._Database = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _Files;

        public bool Files
        {
            get
            {
                return this._Files;
            }
            set
            {
                this._Files = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasDatabase;

        public bool HasDatabase
        {
            get
            {
                return this._HasDatabase;
            }
            set
            {
                this._HasDatabase = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasFile;

        public bool HasFile
        {
            get
            {
                return this._HasFile;
            }
            set
            {
                this._HasFile = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasPlugin;

        public bool HasPlugin
        {
            get
            {
                return this._HasPlugin;
            }
            set
            {
                this._HasPlugin = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _HasTheme;

        public bool HasTheme
        {
            get
            {
                return this._HasTheme;
            }
            set
            {
                this._HasTheme = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _Plugins;

        public bool Plugins
        {
            get
            {
                return this._Plugins;
            }
            set
            {
                this._Plugins = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        private bool _Themes;

        public bool Themes
        {
            get
            {
                return this._Themes;
            }
            set
            {
                this._Themes = value;
                this.RaisePropertyChanged();

                this.RaiseCanExecuteBackupRestoreChanged();
            }
        }

        #endregion

        #region メソッド

        protected abstract bool CanExecute();

        protected abstract bool CanExecuteSelectDirectory();

        protected abstract void Execute();

        protected abstract void ExecuteSelectDirectory();

        #region ヘルパーメソッド

        private void RaiseCanExecuteBackupRestoreChanged()
        {
            if (this.Command != null)
            {
                this.Command.RaiseCanExecuteChanged();
            }
        }

        protected void UpdateStatus()
        {
        }

        #endregion

        #endregion

    }

}
