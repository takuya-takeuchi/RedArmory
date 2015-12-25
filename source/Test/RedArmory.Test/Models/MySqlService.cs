using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class MySqlService
    {


        [TestMethod]
        public void Backup()
        {
            var values = RedArmory.Models.Services.BitnamiRedmineService.Instance.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            const string basePath = @"F:\";
            foreach (var stack in values)
            {
                var configurations = RedArmory.Models.Services.RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                Assert.IsTrue(configurations.Length != 0);

                foreach (var configuration in configurations)
                {
                    var path = Path.Combine(basePath, string.Format("{0}-{1}.sql",configuration.Mode,configuration.Name));
                    RedArmory.Models.Services.MySqlService.Instance.Backup(stack, configuration, path);
                }
            }
        }


        [TestMethod]
        public void Restore()
        {
            var values = RedArmory.Models.Services.BitnamiRedmineService.Instance.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            const string basePath = @"F:\";
            foreach (var stack in values)
            {
                var configurations = RedArmory.Models.Services.RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(stack).ToArray();
                Assert.IsTrue(configurations.Length != 0);

                foreach (var configuration in configurations)
                {
                    var path = Path.Combine(basePath, string.Format("{0}-{1}.sql",configuration.Mode,configuration.Name));
                    RedArmory.Models.Services.MySqlService.Instance.Restore(stack, configuration, path);
                }
            }
        }

    }

}
