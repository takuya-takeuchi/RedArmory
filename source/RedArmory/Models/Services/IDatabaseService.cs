namespace RedArmory.Models.Services
{

    internal interface IDatabaseService
    {
        void Backup(BitnamiRedmineStack stack, DatabaseConfiguration configuration, string path);

        void Restore(BitnamiRedmineStack stack, DatabaseConfiguration configuration, string path);

    }

}