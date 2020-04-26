using Autofac;
using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using System.Collections.Generic;
using System.Linq;
namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class BackupDbJob : BaseIjob
    {
        public BackupDbJob()
           : base(EnumJobs.BackupDB)
        {
        }
         
    }
}