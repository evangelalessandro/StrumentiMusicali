using FluentScheduler;

namespace StrumentiMusicali.Core.Scheduler
{
    public class SchedulerTask 
    {
        public void Init()
        {
            JobManager.AddJob<BackupJob>(a => a.ToRunNow().AndEvery(8).Hours());
        }
    }
}