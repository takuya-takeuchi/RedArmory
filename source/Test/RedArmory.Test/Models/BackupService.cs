using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedArmory.Models;
using RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class BackupService
    {


        [TestMethod]
        public void Backup()
        {
            var stacks = RedArmory.Models.Services.BitNamiRedmineService.Instance.GetBitNamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            foreach (var stack in stacks)
            {
                var targetDirectory = @"F:\RedArmory";
                var directory = DateTime.Now.ToString("yyyyMMdd_hhmmsshhh");
                var path = Path.Combine(targetDirectory, directory);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var configuration = new BackupConfiguration();
                configuration.Themes = true;
                configuration.Files = true;
                configuration.Plugins = true;
                RedArmory.Models.Services.BackupService.Instance.Backup(stack, configuration, path ,new Progress<BackupRestoreProgressReport>());
            }
        }


        [TestMethod]
        public void Restore()
        {
            var stacks = RedArmory.Models.Services.BitNamiRedmineService.Instance.GetBitNamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            var path = @"F:\RedArmory\20151213_03084403";
            foreach (var stack in stacks)
            {
                var configuration = new BackupConfiguration();
                configuration.Themes = true;
                configuration.Files = true;
                configuration.Plugins = true;
                RedArmory.Models.Services.BackupService.Instance.Restore(stack, configuration, path, new Progress<BackupRestoreProgressReport>());
            }
        }

    }

}
