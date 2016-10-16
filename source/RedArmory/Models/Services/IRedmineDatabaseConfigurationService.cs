using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    public interface IRedmineDatabaseConfigurationService
    {

        IEnumerable<DatabaseConfiguration> GetDatabaseConfiguration(BitnamiRedmineStack info);

    }

}