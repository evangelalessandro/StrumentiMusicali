
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using StrumentiMusicali.Library.Repo;
using System;

namespace StrumentiMusicali.ftpBackup.Backup
{
    public class BackupJob : IPlugInJob
    {
        public void Exec()
        {
            try
            {
                var hours = (SettingBackupFtpValidator.ReadSetting().UltimoBackup - DateTime.Now).TotalHours;
                if (Math.Abs(hours) < 8)
                    return;

                using (var uof = new UnitOfWork())
                {
                    uof.EseguiBackup();

                    using (var ftpManager = new ftpBackup.Backup.BackupManager())
                    {
                        ftpManager.Manage();

                        ManagerLog.Logger.Info("Backup Effettuato correttamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ManagerLog.Logger.Error(ex, "Nella fase di backup automatico");
            }
        }

        public EnumJobs Name()
        {
            return EnumJobs.BackupDB;
        }
    }
}