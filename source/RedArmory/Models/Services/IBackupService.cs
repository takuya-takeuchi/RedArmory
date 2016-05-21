using System;

namespace RedArmory.Models.Services
{

    public interface IBackupService
    {
        void Backup(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<BackupRestoreProgressReport> progress);

        BackupConfiguration CheckRestoreFolder(BitnamiRedmineStack stack, string path);

        void Restore(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<BackupRestoreProgressReport> progress);

    }

}