using System;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Models.Services;

namespace RedArmory.Test.Models
{

    [TestClass]
    public class BackupService
    {


        [TestMethod]
        public void Backup()
        {
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var stacks = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            var backupService = SimpleIoc.Default.GetInstance<IBackupService>();
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
                backupService.Backup(stack, configuration, path ,new Progress<ProgressReportsModel>());
            }
        }


        [TestMethod]
        public void Restore()
        {
            var bitnamiRedmineService = SimpleIoc.Default.GetInstance<IBitnamiRedmineService>();

            var stacks = bitnamiRedmineService.GetBitnamiRedmineStacks().ToArray();
            Assert.IsTrue(stacks.Length != 0);

            var backupService = SimpleIoc.Default.GetInstance<IBackupService>();
            var path = @"F:\RedArmory\20151213_03084403";
            foreach (var stack in stacks)
            {
                var configuration = new BackupConfiguration();
                configuration.Themes = true;
                configuration.Files = true;
                configuration.Plugins = true;
                backupService.Restore(stack, configuration, path, new Progress<ProgressReportsModel>());
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            SimpleIoc.Default.Register<ILogService, NLogLogService>();
            SimpleIoc.Default.Register<IDatabaseService, MySqlService>();
            SimpleIoc.Default.Register<IBackupService, BackupService>();
            SimpleIoc.Default.Register<IBitnamiRedmineService, BitnamiRedmineService>();
        }

    }

}
