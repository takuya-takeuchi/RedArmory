using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using Ouranos.RedArmory.Models.Helpers;

namespace Ouranos.RedArmory.Models.Services
{

    internal sealed class BitnamiRedmineService : IBitnamiRedmineService
    {

        #region イベント

        #endregion

        #region フィールド

        private const string ApachePath = @"apache2\bin\httpd.exe";

        private const string MySqlPath = @"mysql\bin\mysqld.exe";

        private const string RedminePath = @"apps\redmine\scripts\winserv.exe";

        private const string SubversionPath = @"subversion\scripts\winserv.exe";

        private readonly ILogService _LogService;

        #endregion

        #region コンストラクタ

        public BitnamiRedmineService(ILogService logService)
        {
            if (logService == null)
                throw new ArgumentNullException(nameof(logService));

            this._LogService = logService;
        }

        #endregion

        #region メソッド

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

        #region IBitnamiRedmineService メンバー

        public IEnumerable<BitnamiRedmineStack> GetBitnamiRedmineStacks()
        {
            var stacks = new List<BitnamiRedmineStack>();

            try
            {
                var registryKey = System.Environment.Is64BitOperatingSystem ?
                    @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" :
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    if (key == null)
                    {
                        throw new KeyNotFoundException($"サブキー 'HKEY_LOCAL_MACHINE\\{registryKey}' が存在しません。");
                    }

                    foreach (var subkeyName in key.GetSubKeyNames())
                    {
                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            var value = subkey?.GetValue("DisplayName") as string;
                            if (value == null)
                            {
                                continue;
                            }

                            if (!value.Equals("Bitnami Redmine Stack", StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }

                            var installLocation = subkey.GetValue("InstallLocation") as string;

                            // Include '"' from 4.0.0
                            installLocation = installLocation?.Trim('"');

                            var displayVersion = subkey.GetValue("DisplayVersion") as string;

                            var stack = new BitnamiRedmineStack(installLocation, displayVersion);
                            stacks.Add(stack);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }

            return stacks;
        }

        public IEnumerable<ServiceStatus> GetServiceDisplayNames(BitnamiRedmineStack stack, ServiceConfiguration configuration)
        {
            var statusList = new List<ServiceStatus>();

            try
            {
                var services = new[]
                {
                    new { Path = Path.Combine(stack.InstallLocation, ApachePath),     Condition = configuration.Apache },
                    new { Path = Path.Combine(stack.InstallLocation, MySqlPath),      Condition = configuration.MySql },
                    new { Path = Path.Combine(stack.InstallLocation, RedminePath),    Condition = configuration.Redmine },
                    new { Path = Path.Combine(stack.InstallLocation, SubversionPath), Condition = configuration.Subversion }
                };

                const string registryKey = @"SYSTEM\CurrentControlSet\services";
                using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    if (key == null)
                        throw new KeyNotFoundException($"サブキー 'HKEY_LOCAL_MACHINE\\{registryKey}' が存在しません。");

                    var subKeyNames = key.GetSubKeyNames();
                    foreach (var subkeyName in subKeyNames)
                    {
                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            var imagePath = subkey?.GetValue("ImagePath") as string;
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
                                    var status = new ServiceStatus(this, subkeyName, startupType);
                                    statusList.Add(status);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }

            return statusList;
        }

        public ServiceStartupType GetStartupType(string displayName)
        {
            try
            {
                var registryKey = System.Environment.Is64BitOperatingSystem ?
                    @"SYSTEM\Wow6432Node\CurrentControlSet\services\{0}" :
                    @"SYSTEM\CurrentControlSet\services\{0}";
                using (var key = Registry.LocalMachine.OpenSubKey(string.Format(registryKey, displayName)))
                {
                    if (key == null)
                        throw new KeyNotFoundException($"サブキー 'HKEY_LOCAL_MACHINE\\{registryKey}' が存在しません。");

                    return GetStartupType(key);
                }
            }
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }
        }

        public void SetStartupType(string displayName, ServiceStartupType startupType)
        {
            try
            {
                //const string registryKey = @"SYSTEM\CurrentControlSet\services\{0}";
                const string registryKey = @"SYSTEM\CurrentControlSet\services\{0}";
                using (var key = Registry.LocalMachine.OpenSubKey(string.Format(registryKey, displayName), true))
                {
                    if (key == null)
                        throw new KeyNotFoundException($"サブキー 'HKEY_LOCAL_MACHINE\\{registryKey}' が存在しません。");

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
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }
        }

        public void StartService(ServiceStatus serviceStatus, ProgressReportsModel report, IProgress<ProgressReportsModel> progress = null)
        {
            try
            {
                var serviceName = serviceStatus.ServiceName;
                using (var sc = new ServiceController(serviceName))
                {
                    try
                    {
                        report.UpdateProgress(serviceName, ProgressState.InProgress);
                        progress?.Report(report);

                        var svcStatus = sc.Status;
                        while (svcStatus != ServiceControllerStatus.Running)
                        {
                            sc.Refresh();

                            svcStatus = sc.Status;

                            this._LogService.Info($"Service status of {serviceStatus.ServiceName} is {svcStatus}");

                            switch (svcStatus)
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

                        report.UpdateProgress(serviceName, ProgressState.Complete);
                        report.AddErrorMessage(serviceName, string.Format(Properties.Resources.Format_SucceedDoService, Properties.Resources.Word_Start));
                    }
                    catch (Exception ex)
                    {
                        report.UpdateProgress(serviceName, ProgressState.Failed);
                        report.AddErrorMessage(serviceName, string.Format(Properties.Resources.Format_FailedDoService, Properties.Resources.Word_Start));

                        this._LogService.Error($"Failed to start '{serviceName}'. Exception is {ex.Message}");
                    }

                    progress?.Report(report);
                }
            }
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }
        }

        public void StopService(ServiceStatus serviceStatus, ProgressReportsModel report, IProgress<ProgressReportsModel> progress = null)
        {
            try
            {
                var serviceName = serviceStatus.ServiceName;
                using (var sc = new ServiceController(serviceName))
                {
                    try
                    {
                        report.UpdateProgress(serviceName, ProgressState.InProgress);
                        progress?.Report(report);

                        var svcStatus = sc.Status;
                        while (svcStatus != ServiceControllerStatus.Stopped)
                        {
                            sc.Refresh();

                            svcStatus = sc.Status;

                            this._LogService.Info($"Service status of {serviceStatus.ServiceName} is {svcStatus}");

                            switch (svcStatus)
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

                        report.UpdateProgress(serviceName, ProgressState.Complete);
                        report.AddErrorMessage(serviceName,
                            string.Format(Properties.Resources.Format_SucceedDoService, Properties.Resources.Word_Stop));
                    }
                    catch (Exception ex)
                    {
                        report.UpdateProgress(serviceName, ProgressState.Failed);
                        report.AddErrorMessage(serviceName,
                            string.Format(Properties.Resources.Format_FailedDoService, Properties.Resources.Word_Stop));

                        this._LogService.Error($"Failed to stop '{serviceName}'. Exception is {ex.Message}");
                    }

                    progress?.Report(report);
                }
            }
            catch (Exception e)
            {
                this._LogService.Error(e.Message);
                throw;
            }
        }

        #endregion

    }

}
