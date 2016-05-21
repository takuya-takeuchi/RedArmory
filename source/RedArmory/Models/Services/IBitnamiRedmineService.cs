using System.Collections.Generic;

namespace RedArmory.Models.Services
{
    public interface IBitnamiRedmineService
    {

        IEnumerable<BitnamiRedmineStack> GetBitnamiRedmineStacks();

        IEnumerable<ServiceStatus> GetServiceDisplayNames(BitnamiRedmineStack stack, ServiceConfiguration configuration);

        ServiceStartupType GetStartupType(string displayName);

        void SetStartupType(string displayName, ServiceStartupType startupType);

        bool StartService(BitnamiRedmineStack stack, ServiceConfiguration configuration);

        bool StopService(BitnamiRedmineStack stack, ServiceConfiguration configuration);

    }
}