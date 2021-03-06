﻿using System;

namespace Ouranos.RedArmory.Models.Services
{

    public interface IBackupService
    {
        void Backup(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<ProgressReportsModel> progress);

        BackupConfiguration CheckRestoreFolder(BitnamiRedmineStack stack, string path);

        void Restore(BitnamiRedmineStack stack, BackupConfiguration configuration, string path, IProgress<ProgressReportsModel> progress);

    }

}