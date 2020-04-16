using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs;
using StrumentiMusicali.Library.Repo;
using System.Linq;

namespace StrumentiMusicali.Core.Scheduler
{
    public class SchedulerTask
    {
        public void Init()
        {
            JobManager.AddJob<BackupJob>(a => a.ToRunNow().AndEvery(8).Hours());
            JobManager.AddJob<IndexOptimizeJob>(a => a.ToRunOnceIn(10).Seconds());
            JobManager.AddJob<UpdateWebJob>(a => a.ToRunNow().AndEvery(1).Minutes());
            JobManager.AddJob<UpdateStockJob>(a => a.ToRunNow().AndEvery(1).Minutes());

            JobManager.JobStart += (o) =>
            {
                ManagerLog.Logger.Info("Avviato job " + o.Name);
                using (var uof = new UnitOfWork())
                {
                    var item = uof.SchedulerJobRepository.Find(a => a.Nome == o.Name).FirstOrDefault();
                    if (item == null)
                    {
                        item = new Library.Entity.Altro.SchedulerJob();
                        item.Nome = o.Name;
                    }
                    item.UltimaEsecuzione = o.StartTime;
                    item.Errore = "";
                    if (item.ID == 0)
                    {
                        uof.SchedulerJobRepository.Add(item);
                    }
                    else
                    {
                        uof.SchedulerJobRepository.Update(item);
                    }
                    uof.Commit();
                }
            };
            JobManager.JobException += (o) =>
            {
                ManagerLog.Logger.Error(o.Exception, "Job " + o.Name);

                using (var uof = new UnitOfWork())
                {
                    var item = uof.SchedulerJobRepository.Find(a => a.Nome == o.Name).FirstOrDefault();
                    if (item == null)
                    {
                        item = new Library.Entity.Altro.SchedulerJob();
                        item.Nome = o.Name;
                    }

                    item.Errore = o.Exception.Message;
                    if (item.ID == 0)
                    {
                        uof.SchedulerJobRepository.Add(item);
                    }
                    else
                    {
                        uof.SchedulerJobRepository.Update(item);
                    }
                    uof.Commit();
                }
            };
            JobManager.JobEnd += (o) =>
            {
                ManagerLog.Logger.Info("Job " + o.Name + " terminato");
                using (var uof = new UnitOfWork())
                {
                    var item = uof.SchedulerJobRepository.Find(a => a.Nome == o.Name).FirstOrDefault();
                    if (item == null)
                    {
                        item = new Library.Entity.Altro.SchedulerJob();
                        item.Nome = o.Name;
                    }
                    item.ProssimoAvvio = o.NextRun.Value;

                    item.UltimaEsecuzione = o.StartTime;
                    item.Duration = o.Duration;
                    if (item.ID == 0)
                    {
                        uof.SchedulerJobRepository.Add(item);
                    }
                    else
                    {
                        uof.SchedulerJobRepository.Update(item);
                    }
                    uof.Commit();
                }
            };
        }
    }
}