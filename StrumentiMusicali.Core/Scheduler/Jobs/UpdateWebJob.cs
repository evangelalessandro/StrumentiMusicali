using Autofac;
using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;

namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class UpdateWebJob : IJob
    {
        public void Execute()
        {
            var updater = IocContainerSingleton.GetContainer.Resolve<IUpdateEcommerce>();
            updater.Exec();
        }
    }
}