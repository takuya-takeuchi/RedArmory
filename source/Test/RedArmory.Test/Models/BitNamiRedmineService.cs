using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class BitNamiRedmineService
    {

        [TestMethod]
        public void GetBitNamiRedmineStacks()
        {
            var stacks = RedArmory.Models.Services.BitNamiRedmineService.Instance.GetBitNamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                Assert.IsNotNull(stack.DisplayVersion);
                Assert.IsNotNull(stack.InstallLocation);
            }
        }

        [TestMethod]
        public void StartService()
        {
            var stacks = RedArmory.Models.Services.BitNamiRedmineService.Instance.GetBitNamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var configuration = new ServiceConfiguration();
                configuration.Redmine = true;

                RedArmory.Models.Services.BitNamiRedmineService.Instance.StartService(stack, configuration);
            }
        }

        [TestMethod]
        public void StopService()
        {
            var stacks = RedArmory.Models.Services.BitNamiRedmineService.Instance.GetBitNamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var configuration = new ServiceConfiguration();
                configuration.Redmine = true;

                RedArmory.Models.Services.BitNamiRedmineService.Instance.StopService(stack, configuration);
            }
        }

    }

}
