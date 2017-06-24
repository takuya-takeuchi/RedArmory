using System;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ouranos.RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class MySqlConnectorService
    {


        [TestMethod]
        public void GetEnumerations()
        {
            var connector = GetDatabaseConnectorService();
            var items = connector.GetEnumerations().ToArray();
            Assert.IsTrue(items.Length != 0);

            foreach (var item in items)
            {
                Console.WriteLine($"Id:{item.Id}, Name:{item.Name}");
            }
        }


        [TestMethod]
        public void GetProjects()
        {
            var connector = GetDatabaseConnectorService();
            var items = connector.GetProjects().ToArray();
            Assert.IsTrue(items.Length != 0);

            foreach (var item in items)
            {
                Console.WriteLine($"Id:{item.Id}, Name:{item.Name}");
            }
        }

        private static IDatabaseConnectorService GetDatabaseConnectorService()
        {
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();
            var redmineDatabaseConfigurationService = SimpleIoc.Default.GetInstance<IRedmineDatabaseConfigurationService>();
            var logService = SimpleIoc.Default.GetInstance<ILogService>();

            var values = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            var databaseConfigurations = redmineDatabaseConfigurationService.GetDatabaseConfiguration(values.FirstOrDefault()).ToArray();
            Assert.IsTrue(databaseConfigurations.Length != 0);

            return new Ouranos.RedArmory.Models.Services.MySqlConnectorService(databaseConfigurations.FirstOrDefault(), logService);
        }

        [TestInitialize]
        public void Initialize()
        {
            SimpleIoc.Default.Register<ILogService, NLogLogService>();
            SimpleIoc.Default.Register<IDatabaseService, Ouranos.RedArmory.Models.Services.MySqlService>();
            SimpleIoc.Default.Register<IBackupService, Ouranos.RedArmory.Models.Services.BackupService>();
            SimpleIoc.Default.Register<IBitnamiRedmineService, Ouranos.RedArmory.Models.Services.BitnamiRedmineService>();
            SimpleIoc.Default.Register<IRedmineDatabaseConfigurationService, Ouranos.RedArmory.Models.Services.RedmineDatabaseConfigurationService>();
        }

    }

}
