using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class MySqlService
    {


        [TestMethod]
        public void Backup()
        {
            var databaseService = SimpleIoc.Default.GetInstance<IDatabaseService>();
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var values = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            const string basePath = @"F:\";
            foreach (var stack in values)
            {
                var configurations = RedArmory.Models.Services.RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                Assert.IsTrue(configurations.Length != 0);

                foreach (var configuration in configurations)
                {
                    var path = Path.Combine(basePath, $"{configuration.Mode}-{configuration.Name}.sql");
                    databaseService.Backup(stack, configuration, path);
                }
            }
        }


        [TestMethod]
        public void Restore()
        {
            var databaseService = SimpleIoc.Default.GetInstance<IDatabaseService>();
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var values = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            const string basePath = @"F:\";
            foreach (var stack in values)
            {
                var configurations = RedArmory.Models.Services.RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                Assert.IsTrue(configurations.Length != 0);

                foreach (var configuration in configurations)
                {
                    var path = Path.Combine(basePath, $"{configuration.Mode}-{configuration.Name}.sql");
                    databaseService.Restore(stack, configuration, path);
                }
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            SimpleIoc.Default.Register<ILoggerService, LoggerService>();
            SimpleIoc.Default.Register<IDatabaseService, MySqlService>();
            SimpleIoc.Default.Register<IBackupService, BackupService>();
            SimpleIoc.Default.Register<IBitnamiRedmineService, BitnamiRedmineService>();
        }

    }

}
