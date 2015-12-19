using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using RedArmory.Models.Helpers;

namespace RedArmory.Models.Services
{

    internal sealed class BitNamiRedmineService
    {

        #region イベント

        #endregion

        #region フィールド

        private const string ApachePath = @"apache2\bin\httpd.exe";

        private const string MySqlPath = @"mysql\bin\mysqld.exe";

        private const string RedminePath = @"apps\redmine\scripts\winserv.exe";

        private const string SubversionPath = @"subversion\scripts\winserv.exe";

        #endregion

        #region コンストラクタ

        static BitNamiRedmineService()
        {
        }

        private BitNamiRedmineService()
        {
        }

        #endregion

        #region プロパティ

        private static BitNamiRedmineService _Instance;

        public static BitNamiRedmineService Instance
        {
            get
            {
                return _Instance ?? (_Instance = new BitNamiRedmineService());
            }
        }

        #endregion

        #region メソッド

        public IEnumerable<BitNamiRedmineStack> GetBitNamiRedmineStacks()
        {
            //const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key == null)
                {
                    throw new KeyNotFoundException(string.Format("サブキー 'HKEY_LOCAL_MACHINE\\{0}' が存在しません。", registryKey));
                }

                foreach (var subkeyName in key.GetSubKeyNames())
                {
                    using (var subkey = key.OpenSubKey(subkeyName))
                    {
                        if (subkey == null)
                        {
                            continue;
                        }

                        var value = subkey.GetValue("DisplayName") as string;
                        if (value == null)
                        {
                            continue;
                        }

                        if (!value.Equals("Bitnami Redmine Stack", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        var installLocation = subkey.GetValue("InstallLocation") as string;
                        var displayVersion = subkey.GetValue("DisplayVersion") as string;

                        yield return new BitNamiRedmineStack(installLocation, displayVersion);
                    }
                }
            }
        }

        public IEnumerable<ServiceStatus> GetServiceDisplayNames(BitNamiRedmineStack stack, ServiceConfiguration configuration)
        {
            var services = new[]
            {
                new
                {
                    Path = Path.Combine(stack.InstallLocation, ApachePath), Condition = configuration.Apache
                },
                new
                {
                    Path = Path.Combine(stack.InstallLocation, MySqlPath), Condition = configuration.MySql
                },
                new
                {
                    Path = Path.Combine(stack.InstallLocation, RedminePath), Condition = configuration.Redmine
                },
                new
                {
                    Path = Path.Combine(stack.InstallLocation, SubversionPath), Condition = configuration.Subversion
                },
            };

            const string registryKey = @"SYSTEM\CurrentControlSet\services";
            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key == null)
                {
                    throw new KeyNotFoundException(string.Format("サブキー 'HKEY_LOCAL_MACHINE\\{0}' が存在しません。", registryKey));
                }

                var subKeyNames = key.GetSubKeyNames();
                foreach (var subkeyName in subKeyNames)
                {
                    using (var subkey = key.OpenSubKey(subkeyName))
                    {
                        if (subkey == null)
                        {
                            continue;
                        }

                        var imagePath = subkey.GetValue("ImagePath") as string;
                        if (string.IsNullOrWhiteSpace(imagePath))
                        {
                            continue;
                        }

                        var displayName = subkey.GetValue("DisplayName") as string;
                        if (string.IsNullOrWhiteSpace(displayName))
                        {
                            continue;
                        }

                        // 時々、8.3 形式の短いパスがある
                        // さらに、引数を含むパスがいるため、分解してから長い形式のパスに変換する
                        // が、長い形式のパスを System.IO.Path.GetFullPath で
                        // 変関すると例外を投げるので、Win32 API の GetLongPathName を使う
                        try
                        {
                            var splitArgs = InteropHelper.SplitArgs(imagePath);
                            if (splitArgs == null || splitArgs.Length == 0)
                            {
                                continue;
                            }

                            imagePath = InteropHelper.GetLongPathName(splitArgs[0]);
                        }
                        catch
                        { }

                        if (imagePath == null)
                        {
                            continue;
                        }

                        foreach (var service in services)
                        {
                            if (!service.Condition)
                            {
                                continue;
                            }

                            if (imagePath.Contains(service.Path))
                            {
                                var startupType = GetStartupType(subkey);
                                yield return new ServiceStatus(subkeyName, startupType);
                            }
                        }
                    }
                }
            }
        }

        public ServiceStartupType GetStartupType(string displayName)
        {
            //const string registryKey = @"SYSTEM\CurrentControlSet\services\{0}";
            const string registryKey = @"SYSTEM\Wow6432Node\CurrentControlSet\services\{0}";
            using (var key = Registry.LocalMachine.OpenSubKey(string.Format(registryKey, displayName)))
            {
                if (key == null)
                {
                    throw new KeyNotFoundException(string.Format("サブキー 'HKEY_LOCAL_MACHINE\\{0}' が存在しません。", registryKey));
                }

                return GetStartupType(key);
            }
        }

        public void SetStartupType(string displayName, ServiceStartupType startupType)
        {
            //const string registryKey = @"SYSTEM\CurrentControlSet\services\{0}";
            const string registryKey = @"SYSTEM\CurrentControlSet\services\{0}";
            using (var key = Registry.LocalMachine.OpenSubKey(string.Format(registryKey, displayName), true))
            {
                if (key == null)
                {
                    throw new KeyNotFoundException(string.Format("サブキー 'HKEY_LOCAL_MACHINE\\{0}' が存在しません。", registryKey));
                }

                switch (startupType)
                {
                    case ServiceStartupType.Disabled:
                        key.SetValue("Start", 4, RegistryValueKind.DWord);
                        return;
                    case ServiceStartupType.Manual:
                        key.SetValue("Start", 3, RegistryValueKind.DWord);
                        return;
                    case ServiceStartupType.Automatic:
                        key.SetValue("Start", 2, RegistryValueKind.DWord);
                        key.SetValue("DelayedAutostart", 0, RegistryValueKind.DWord);
                        return;
                    case ServiceStartupType.DelayStart:
                        key.SetValue("Start", 2, RegistryValueKind.DWord);
                        key.SetValue("DelayedAutostart", 1, RegistryValueKind.DWord);
                        return;
                }
            }
        }

        public bool StartService(BitNamiRedmineStack stack, ServiceConfiguration configuration)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("stack");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            foreach (var displayName in GetServiceDisplayNames(stack, configuration))
            {
                using (var sc = new ServiceController(displayName.ServiceName))
                {
                    sc.Refresh();

                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }

                    var svcStatus = sc.Status;
                    while (svcStatus != ServiceControllerStatus.Running)
                    {
                        sc.Refresh();

                        switch (sc.Status)
                        {
                            case ServiceControllerStatus.Paused:
                                sc.Continue();
                                break;
                            case ServiceControllerStatus.Stopped:
                                sc.Start();
                                break;
                            case ServiceControllerStatus.ContinuePending:
                            case ServiceControllerStatus.PausePending:
                            case ServiceControllerStatus.StartPending:
                            case ServiceControllerStatus.StopPending:
                                break;
                        }

                        svcStatus = sc.Status;
                    }
                }
            }

            return true;
        }

        public bool StopService(BitNamiRedmineStack stack, ServiceConfiguration configuration)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("stack");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            foreach (var displayName in GetServiceDisplayNames(stack, configuration))
            {
                using (var sc = new ServiceController(displayName.ServiceName))
                {
                    sc.Refresh();

                    sc.Stop();

                    var svcStatus = sc.Status;
                    while (svcStatus != ServiceControllerStatus.Stopped)
                    {
                        sc.Refresh();

                        switch (sc.Status)
                        {
                            case ServiceControllerStatus.Paused:
                                sc.Continue();
                                break;
                            case ServiceControllerStatus.Running:
                                sc.Stop();
                                break;
                            case ServiceControllerStatus.ContinuePending:
                            case ServiceControllerStatus.PausePending:
                            case ServiceControllerStatus.StartPending:
                            case ServiceControllerStatus.StopPending:
                                break;
                        }

                        svcStatus = sc.Status;
                    }
                }
            }

            return true;
        }

        #region ヘルパーメソッド

        private static ServiceStartupType GetStartupType(RegistryKey subKey)
        {
            var start = (int)subKey.GetValue("Start");
            switch (start)
            {
                case 2:
                    var value = subKey.GetValue("DelayedAutostart");
                    if (value == null)
                    {
                        return ServiceStartupType.Automatic;
                    }

                    return (int)value == 1 ? ServiceStartupType.DelayStart : ServiceStartupType.Automatic;
                case 3:
                    return ServiceStartupType.Manual;
                case 4:
                    return ServiceStartupType.Disabled;
            }

            return ServiceStartupType.Unknown;
        }

        #endregion

        #endregion

    }

}
