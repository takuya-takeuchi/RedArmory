using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Yaml.Serialization;

namespace RedArmory.Models.Services
{

    internal sealed class RedmineDatabaseConfigurationService : IRedmineDatabaseConfigurationService
    {

        #region イベント
        #endregion

        #region フィールド

        private readonly ILoggerService _LoggerService;

        #endregion

        #region コンストラクタ

        public RedmineDatabaseConfigurationService(ILoggerService loggerService)
        {
            if (loggerService == null)
                throw new ArgumentNullException(nameof(loggerService));

            this._LoggerService = loggerService;
        }

        #endregion

        #region メソッド

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

        #region IRedmineDatabaseConfigurationService メンバー

        public IEnumerable<DatabaseConfiguration> GetDatabaseConfiguration(BitnamiRedmineStack info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

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


                foreach (var kvp in dictionary)
                {
                    var mode = kvp.Key as string;
                    if (mode == null)
                    {
                        continue;
                    }

                    var values = kvp.Value as Dictionary<object, object>;
                    if (values == null)
                    {
                        continue;
                    }

                    var database = values["database"] as string;
                    var username = values["username"] as string;
                    var password = values["password"] as string;
                    var encoding = values["encoding"] as string;

                    yield return new DatabaseConfiguration(mode, database, username, password, encoding, port);
                }
            }
        }

        #endregion

    }
}
