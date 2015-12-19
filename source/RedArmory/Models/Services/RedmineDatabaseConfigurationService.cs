using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Yaml.Serialization;

namespace RedArmory.Models.Services
{
    internal sealed class RedmineDatabaseConfigurationService
    {


        #region イベント
        #endregion

        #region フィールド
        #endregion

        #region コンストラクタ

        static RedmineDatabaseConfigurationService()
        {
        }

        private RedmineDatabaseConfigurationService()
        {
        }

        #endregion

        #region プロパティ

        private static RedmineDatabaseConfigurationService _Instance;

        public static RedmineDatabaseConfigurationService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new RedmineDatabaseConfigurationService());
            }
        }

        #endregion

        #region メソッド

        public IEnumerable<DatabaseConfiguration> GetDatabaseConfiguration(BitNamiRedmineStack info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            const string databaseYmlPath = @"apps\redmine\htdocs\config\database.yml";
            var path = Path.Combine(info.InstallLocation, databaseYmlPath);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("database.yml が存在しません。", path);
            }

            object[] deserialized = null;

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
                foreach (var values in (from kvp in dictionary let mode = kvp.Key as string
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

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド
        #endregion

        #endregion

    }
}
