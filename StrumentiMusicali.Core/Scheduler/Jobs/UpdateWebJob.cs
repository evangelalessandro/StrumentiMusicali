﻿using Autofac;
using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using System.Collections.Generic;
using System.Linq;
namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class UpdateWebJob : IJob
    {
         
        public void Execute()
        { 
            var updater = IocContainerSingleton.GetContainer.Resolve<IEnumerable<IPlugInJob>>().Where(a=>a.Name()==EnumJobs.UpdateWeb).FirstOrDefault();
            
            updater.Exec();
        }
    }
}