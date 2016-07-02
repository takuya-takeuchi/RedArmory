using System.Collections.Generic;

namespace Ouranos.RedArmory.Models.Services
{

    internal interface IRedmineDatabaseConfigurationService
    {

        IEnumerable<DatabaseConfiguration> GetDatabaseConfiguration(BitnamiRedmineStack info);

    }

}