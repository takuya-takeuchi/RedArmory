using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    public interface IDatabaseConnectorService
    {

        IEnumerable<EnumerationItem> GetEnumerations();

        IEnumerable<ProjectItem> GetProjects();

        void UpdateEnumerations(IEnumerable<EnumerationItem> items);

    }

}