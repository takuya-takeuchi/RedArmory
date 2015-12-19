using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedArmory.Test.Models
{

    [TestClass]
    public  class RedmineDatabaseConfigurationService
    {


        [TestMethod]
        public void GetDatabaseConfiguration()
        {
            var values = RedArmory.Models.Services.BitNamiRedmineService.Instance.GetBitNamiRedmineStacks().ToArray();
            Assert.IsTrue(values.Length != 0);

            foreach (var serviceInfo in values)
            {
                var configurations = RedArmory.Models.Services.RedmineDatabaseConfigurationService.Instance.GetDatabaseConfiguration(serviceInfo).ToArray();
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

    }

}
