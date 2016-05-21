using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace RedArmory.Models.Services
{

    public sealed class ApplicationSettingService
    {

        #region フィールド

        private readonly string _ApplicationSettingDirectory;

        private readonly string _ApplicationSettingPath;

        private ApplicationSetting _ApplicationSetting;

        #endregion

        #region コンストラクタ

        public ApplicationSettingService()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this._ApplicationSettingDirectory = Path.Combine(path, AssemblyProperty.Product);
            this._ApplicationSettingPath = Path.Combine(this._ApplicationSettingDirectory, "Setting.xml");

            this._BackupHistories = new ObservableCollection<BackupHistorySetting>();
        }

        #endregion

        #region プロパティ

        private static ApplicationSettingService _Instance;

        public static ApplicationSettingService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new ApplicationSettingService());
            }
        }

        private readonly ObservableCollection<BackupHistorySetting> _BackupHistories;

        public ObservableCollection<BackupHistorySetting> BackupHistories
        {
            get
            {
                return this._BackupHistories;
            }
        }

        #endregion

        #region メソッド

        public ApplicationSetting GetApplicationSetting()
        {
            if (this._ApplicationSetting != null)
            {
                return this._ApplicationSetting;
            }

            var applicationSettingPath = this._ApplicationSettingPath;
            if (File.Exists(applicationSettingPath))
            {
                var serializer = new DataContractSerializer(typeof(ApplicationSetting));
                using (var reader = XmlReader.Create(applicationSettingPath))
                {
                    this._ApplicationSetting = (ApplicationSetting)serializer.ReadObject(reader);
                }
            }
            else
            {
                this._ApplicationSetting = new ApplicationSetting();
            }

            foreach (var backupHistorySetting in this._ApplicationSetting.BackupHistories)
            {
                this._BackupHistories.Add(backupHistorySetting);
            }

            return this._ApplicationSetting;
        }

        public void UpdateApplicationSetting(ApplicationSetting applicationSetting)
        {
            if (applicationSetting == null)
                throw new ArgumentNullException(nameof(applicationSetting));

            var applicationSettingDirectory = this._ApplicationSettingDirectory;
            Directory.CreateDirectory(applicationSettingDirectory);

            var applicationSettingPath = this._ApplicationSettingPath;
            var serializer = new DataContractSerializer(typeof(ApplicationSetting));
            using (var writer = XmlWriter.Create(applicationSettingPath))
            {
                serializer.WriteObject(writer, applicationSetting);
            }

            this._ApplicationSetting = applicationSetting;
        }

        #endregion

    }

}
