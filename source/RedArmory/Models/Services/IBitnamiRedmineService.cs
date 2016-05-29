using System;
using System.Collections.Generic;

namespace RedArmory.Models.Services
{
    public interface IBitnamiRedmineService
    {

        IEnumerable<BitnamiRedmineStack> GetBitnamiRedmineStacks();

        IEnumerable<ServiceStatus> GetServiceDisplayNames(BitnamiRedmineStack stack, ServiceConfiguration configuration);

        ServiceStartupType GetStartupType(string displayName);

        void SetStartupType(string displayName, ServiceStartupType startupType);

        void StartService(ServiceStatus serviceStatus, ProgressReportsModel report, IProgress<ProgressReportsModel> progress = null);

        void StopService(ServiceStatus serviceStatus, ProgressReportsModel report, IProgress<ProgressReportsModel> progress = null);

    }

}