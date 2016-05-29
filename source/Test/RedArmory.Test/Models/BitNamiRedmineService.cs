using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class BitnamiRedmineService
    {

        [TestMethod]
        public void GetBitnamiRedmineStacks()
        {
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var stacks = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
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
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var stacks = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var configuration = new ServiceConfiguration
                {
                    Redmine = true
                };

                foreach (var serviceStatuse in bitnamiRedmineService.GetServiceDisplayNames(stack, configuration))
                {
                    bitnamiRedmineService.StartService(serviceStatuse, new ProgressReportsModel(new[] { new ProgressItemModel() }));
                }
            }
        }

        [TestMethod]
        public void StopService()
        {
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var stacks = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var configuration = new ServiceConfiguration
                {
                    Redmine = true
                };

                foreach (var serviceStatuse in bitnamiRedmineService.GetServiceDisplayNames(stack, configuration))
                {
                    bitnamiRedmineService.StopService(serviceStatuse, new ProgressReportsModel(new[] { new ProgressItemModel()}));
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
