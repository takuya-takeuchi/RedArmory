using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using Ouranos.RedArmory.Models.DatabaseObjects;

namespace Ouranos.RedArmory.Models.Services
{
    internal sealed class MySqlConnectorService : IDatabaseConnectorService
    {

        #region フィールド

        private readonly DatabaseConfiguration _DatabaseConfiguration;

        #endregion

        #region コンストラクタ

        public MySqlConnectorService(DatabaseConfiguration coniConfiguration)
        {
            this._DatabaseConfiguration = coniConfiguration;
        }

        #endregion

        #region プロパティ
        #endregion

        #region メソッド

        #region オーバーライド
        #endregion

        #region イベントハンドラ
        #endregion

        #region ヘルパーメソッド

        private string CreateConnectionString()
        {
            var configuration = this._DatabaseConfiguration;
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = (uint)configuration.Port,
                UserID = configuration.Username,
                Password = configuration.Password,
                Database = configuration.Name
            };
            return builder.ToString();
        }

        private IEnumerable<T> Query<T>(string table)
        {
            using (var connection = new MySqlConnection(this.CreateConnectionString()))
            {
                connection.Open();
                return connection.Query<T>($"SELECT * FROM {table}");
            }
        }

        private void Update<T>(string query, IEnumerable<T> items)
        {
            using (var connection = new MySqlConnection(this.CreateConnectionString()))
            {
                connection.Open();
                using (var tran = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in items)
                            connection.Execute(query, item, tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region IDatabaseConnectorService メンバー

        public IEnumerable<EnumerationItem> GetEnumerations()
        {
            return this.Query<EnumerationObject>("enumerations").Select(item => new EnumerationItem(item));
        }

        public IEnumerable<ProjectItem> GetProjects()
        {
            return this.Query<ProjectObject>("projects").Select(item => new ProjectItem(item));
        }

        public void UpdateEnumerations(IEnumerable<EnumerationItem> items)
        {
            var set = new[]
            {
                "id=@Id",
                "name=@Name",
                "position=@Position",
                "is_default=@Is_Default",
                "type=@Type",
                "active=@Active",
                "project_id=@Project_Id",
                "parent_id=@Parent_Id",
                "position_name=@Position_Name",
            };
            var @where = "WHERE id=@Id";

            var query = $"UPDATE enumerations SET {string.Join(", ", set)} {@where}";
            this.Update(query, items.Select(item => new EnumerationObject(item)));
        }

        #endregion

    }

}