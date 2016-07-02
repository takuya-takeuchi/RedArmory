using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ouranos.RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public  class RedmineDatabaseConfigurationService
    {


        [TestMethod]
        public void GetDatabaseConfiguration()
        {
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();
            var redmineDatabaseConfigurationService = SimpleIoc.Default.GetInstance<IRedmineDatabaseConfigurationService>();

            var values = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            foreach (var serviceInfo in values)
            {
                var configurations = redmineDatabaseConfigurationService.GetDatabaseConfiguration(serviceInfo).ToArray();
                Assert.IsTrue(configurations.Length != 0);

                foreach (var configuration in configurations)
                {
                    Assert.IsNotNull(configuration.Mode);
                    Assert.IsNotNull(configuration.Name);
                    Assert.IsNotNull(configuration.Username);
                    Assert.IsNotNull(configuration.Password);
                    Assert.IsNotNull(configuration.Encoding);
                    Assert.IsTrue(configuration.Port > 0);
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
            SimpleIoc.Default.Register<IRedmineDatabaseConfigurationService, RedmineDatabaseConfigurationService>();
        }

    }

}
