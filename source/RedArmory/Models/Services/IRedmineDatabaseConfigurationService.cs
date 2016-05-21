using System.Collections.Generic;

namespace RedArmory.Models.Services
{

    internal interface IRedmineDatabaseConfigurationService
    {

        IEnumerable<DatabaseConfiguration> GetDatabaseConfiguration(BitnamiRedmineStack info);

    }

}