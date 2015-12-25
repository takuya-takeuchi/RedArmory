using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using RedArmory.Models.Configurations;

namespace RedArmory.Models
{

    public sealed class ConfigurationService
    {

        #region フィールド

        private static string SettingFileName = "setting.config";

        private ApplicationConfiguration Configuration = null;

        #endregion

        #region コンストラクタ

        static ConfigurationService()
        { }

        private ConfigurationService()
        {
        }

        #endregion

        #region プロパティ

        private static ConfigurationService _Instance;

        public static ConfigurationService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new ConfigurationService());
            }
        }

        #endregion

        #region メソッド

        public BitnamiRedmineStackConfiguration GetBitnamiRedmineStackConfiguration(string version)
        {
            if (this.Configuration == null)
            {
                this.LoadConfiguratuion();
            }

            var configuration = this.Configuration;
            var array = configuration.BitnamiRedmineStackConfiguration ?? new BitnamiRedmineStackConfiguration[0];
            var stackConfiguration = array.FirstOrDefault(c => c.Version.Equals(version));
            if (stackConfiguration != null)
            {
                return stackConfiguration;
            }

            stackConfiguration = new BitnamiRedmineStackConfiguration();
            stackConfiguration.Version = version;

            var list = new List<BitnamiRedmineStackConfiguration>(array);
            list.Add(stackConfiguration);

            configuration.BitnamiRedmineStackConfiguration = list.ToArray();
            return stackConfiguration;
        }

        public void LoadConfiguratuion()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, SettingFileName);

            ApplicationConfiguration configuration = null;

            try
            {
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var serializer = new XmlSerializer(typeof(ApplicationConfiguration));
                        configuration = (ApplicationConfiguration)serializer.Deserialize(fs);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (configuration == null)
                {
                    configuration = new ApplicationConfiguration();
                }
            }

            this.Configuration = configuration;
        }

        public void SaveConfiguration()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, SettingFileName);

            try
            {
                using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    var serializer = new XmlSerializer(typeof(ApplicationConfiguration));
                    serializer.Serialize(fs, this.Configuration);
                }
            }
            catch (Exception)
            {
            }
        }

        #region イベントハンドラ

        #endregion

        #region プライベート

        #endregion

        #endregion


    }

}
