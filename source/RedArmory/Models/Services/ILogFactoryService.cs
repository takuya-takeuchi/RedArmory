using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    public interface ILogFactoryService
    {

        ILogService Create();

        ILogService Create(string name);

        ILogService Create(string name, IDictionary<string, string> variables);

    }

}
