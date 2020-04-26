using Autofac;
using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class BaseIjob : IJob
    {
        private EnumJobs enTipo { get; set; }
        public BaseIjob(EnumJobs enumJobs)
        {
            enTipo = enumJobs;
        }

        static bool inProgress = false;
        public void Execute()
        {
            if (inProgress)
                return;
            inProgress = true;
            try
            {
                var updater = IocContainerSingleton.GetContainer.Resolve<IEnumerable<IPlugInJob>>()
                    .Where(a => a.Name() == enTipo).FirstOrDefault();
                updater.Exec();
            }
            finally
            {
                inProgress = false;
            }

        }
    }
}
