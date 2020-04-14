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

            JobManager.AddJob<UpdateWebJob>(a => a.ToRunNow().AndEvery(1).Minutes());

            JobManager.JobStart += (o) =>
            {
                ManagerLog.AddLogMessage("Avviato job " + o.Name);
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
                ManagerLog.AddLogException("Job " + o.Name, o.Exception);

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
                ManagerLog.AddLogMessage("Job " + o.Name + " terminato");
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