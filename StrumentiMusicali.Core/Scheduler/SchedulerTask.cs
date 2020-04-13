using Autofac;
using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using System;

namespace StrumentiMusicali.Core.Scheduler
{
    public class SchedulerTask
    {
        public void Init()
        {
            //JobManager.AddJob<BackupJob>(a => a.ToRunNow().AndEvery(8).Hours());


            JobManager.AddJob(new System.Action(UpdateWeb), a => a.ToRunNow().AndEvery(1).Minutes());
            
        }


        private void UpdateWeb()
        {
            
            var updater = IocContainerSingleton.GetContainer.Resolve<IUpdateEcommerce>();
            updater.Exec();
        }
    }
}