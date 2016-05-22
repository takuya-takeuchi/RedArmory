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
        
        bool ControlService(BitnamiRedmineStack stack, ServiceConfiguration configuration,IProgress<ProgressReportsModel> progress = null);

    }
}