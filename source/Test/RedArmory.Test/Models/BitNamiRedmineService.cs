using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class BitnamiRedmineService
    {

        [TestMethod]
        public void GetBitnamiRedmineStacks()
        {
            var stacks = RedArmory.Models.Services.BitnamiRedmineService.Instance.GetBitnamiRedmineStacks().ToArray();
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
            var stacks = RedArmory.Models.Services.BitnamiRedmineService.Instance.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var configuration = new ServiceConfiguration();
                configuration.Redmine = true;

                RedArmory.Models.Services.BitnamiRedmineService.Instance.StartService(stack, configuration);
            }
        }

        [TestMethod]
        public void StopService()
        {
            var stacks = RedArmory.Models.Services.BitnamiRedmineService.Instance.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var configuration = new ServiceConfiguration();
                configuration.Redmine = true;

                RedArmory.Models.Services.BitnamiRedmineService.Instance.StopService(stack, configuration);
            }
        }

    }

}
