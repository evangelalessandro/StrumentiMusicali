using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs;
using StrumentiMusicali.Library.Repo;
using System.Linq;
using System.Timers;

namespace StrumentiMusicali.Core.Scheduler
{
    public class SchedulerTask
    {
        public void Init()
        {
            var obj = IocContainerSingleton.GetContainer;
            ManagerLog.Logger.Info("Init Scheduler");
            JobManager.AddJob<BackupDbJob>(a => a.ToRunOnceAt(20, 0).AndEvery(1).Days());
            JobManager.AddJob<IndexOptimizeJob>(a => a.ToRunEvery(12).Hours());
            JobManager.AddJob<UpdateWebJob>(a => a.ToRunEvery(1).Minutes());
            JobManager.AddJob<UpdateStockJob>(a => a.ToRunEvery(1).Minutes());
            JobManager.Stop();

            InitDbRecord();

            JobManager.Start();

            Timer timer = new Timer();
            timer.Interval = 15000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
            
            AttachEvents();

            ManagerLog.Logger.Info("Fine Init Scheduler");
        }

        private static void AttachEvents()
        {
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

        private void InitDbRecord()
        {
            var list = JobManager.AllSchedules.ToList().Select(a=>new { a.Name, a.NextRun }).Distinct().ToList();
            using (var uof = new UnitOfWork())
            {

                foreach (var o in list)
                {
                    var item = uof.SchedulerJobRepository.Find(a => a.Nome == o.Name).FirstOrDefault();
                    if (item == null)
                    {
                        item = new Library.Entity.Altro.SchedulerJob();
                        item.Nome = o.Name;
                    }
                    item.ProssimoAvvio = list.Where(a=>a.Name==o.Name).Min(a=>a.NextRun);
                    if (item.ID == 0)
                    {
                        item.Errore = "";
                        item.Enabled = false;
                        uof.SchedulerJobRepository.Add(item);
                    }
                    else
                    {
                        uof.SchedulerJobRepository.Update(item);
                    }

                    uof.Commit();
                }
            }

            ReadUpdateDb();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReadUpdateDb();
        }

        private void ReadUpdateDb()
        {
            using (var uof = new UnitOfWork())
            {
                var items = uof.SchedulerJobRepository.Find(a => 1 == 1).ToList();
                foreach (var item in items)
                {
                    var schedItem = JobManager.GetSchedule(item.Nome);

                    if (item.Enabled)
                    {
                        if (schedItem.Disabled)
                        {
                            schedItem.Enable();
                        }
                    }
                    else
                    {
                        if (!schedItem.Disabled)
                            schedItem.Disable();
                    }
                }
            }
        }
    }
}