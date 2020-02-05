using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Yaml.Serialization;

namespace Ouranos.RedArmory.Models.Services
{

    internal sealed class RedmineDatabaseConfigurationService : IRedmineDatabaseConfigurationService
    {

        #region Fields

        private readonly ILogService _LogService;

        #endregion

        #region Constructors

        public RedmineDatabaseConfigurationService(ILogService logService)
        {
            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            this._LogService = logService;
        }

        #endregion

        #region Methods

        #region Overrides
        #endregion

        #region Event Handlers
        #endregion

        #region Helpers
        #endregion

        #endregion

        #region IRedmineDatabaseConfigurationService Members

        public IEnumerable<DatabaseConfiguration> GetDatabaseConfiguration(BitnamiRedmineStack info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            try
            {
                const string databaseYmlPath = @"apps\redmine\htdocs\config\database.yml";
                var path = Path.Combine(info.InstallLocation, databaseYmlPath);
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("database.yml が存在しません。", path);
                }

                object[] deserialized;

                var serializer = new YamlSerializer();
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    deserialized = serializer.Deserialize(fs);
                }

                var configurations = new List<DatabaseConfiguration>();
                foreach (var obj in deserialized)
                {
                    var dictionary = obj as Dictionary<object, object>;
                    if (dictionary == null)
                    {
                        continue;
                    }

                    // ポート番号は 1 つしか存在しない
                    var port = 0;
                    foreach (var values in (from kvp in dictionary
                                            let mode = kvp.Key as string
                                            where mode != null
                                            select kvp.Value).OfType<Dictionary<object, object>>().Where(values => values.ContainsKey("port")))
                    {
                        port = (int)values["port"];
                    }

                    configurations.AddRange(from kvp in dictionary let mode = kvp.Key as string
                                            where mode != null let values = kvp.Value as Dictionary<object, object>
                                            where values != null
                                            let database = values["database"] as string
                                            let username = values["username"] as string
                                            let password = values["password"] as string
                                            let encoding = values["encoding"] as string
                                            let host = values["host"] as string
                                            select new DatabaseConfiguration(mode, database, host, username, password, encoding, port));
                }

                return configurations;
            }
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }
        }

        #endregion

    }
}
