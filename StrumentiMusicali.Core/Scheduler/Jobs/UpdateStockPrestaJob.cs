﻿using Autofac;
using FluentScheduler;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.Core.Scheduler.Jobs
{
    public class UpdateLocalStockPrestaJob : BaseIjob
    {
        public UpdateLocalStockPrestaJob() 
            : base(EnumJobs.PrestaUpdateFromWeb)
        {

        }
    }
}