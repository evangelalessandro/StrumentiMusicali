using FluentScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Scheduler.Jobs.Interface
{ 
    public interface IUpdateEcommerce 
    {
        void Exec();
    }
}
