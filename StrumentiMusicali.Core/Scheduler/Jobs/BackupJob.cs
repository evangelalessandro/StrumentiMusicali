using FluentScheduler;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Repo;
using System;

namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class BackupJob : IJob
    {
        public void Execute()
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
                ManagerLog.Logger.Error(ex,"Nella fase di backup automatico");
            }
        }
    }
}