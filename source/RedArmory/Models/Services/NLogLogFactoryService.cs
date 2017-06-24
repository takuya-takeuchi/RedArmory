using System.Collections.Generic;
using NLog;

namespace Ouranos.RedArmory.Models.Services
{

    public sealed class NLogLogFactoryService : ILogFactoryService
    {

        #region ILogFactoryService メンバー

        public ILogService Create()
        {
            return new NLogLogService(LogManager.GetCurrentClassLogger());
        }

        public ILogService Create(string name)
        {
            return new NLogLogService(LogManager.GetLogger(name));
        }

        public ILogService Create(string name, IDictionary<string, string> variables)
        {
            return new NLogLogService(name, variables);
        }

        #endregion

    }

}
