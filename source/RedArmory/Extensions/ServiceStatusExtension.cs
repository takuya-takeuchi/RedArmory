using Ouranos.RedArmory.Models;
using Ouranos.RedArmory.Properties;

namespace Ouranos.RedArmory.Extensions
{

    internal static class ServiceStatusExtension
    {

        public static ProgressItemModel ToProgressItemModel(this ServiceStatus source, bool requiredStart)
        {
            return new ProgressItemModel
            {
                TaskName = string.Format(requiredStart ? Resources.Format_StartService : Resources.Format_StopService, source.ServiceName),
                Key = source.ServiceName,
                Progress = ProgressState.NotStart
            };
        }

    }

}