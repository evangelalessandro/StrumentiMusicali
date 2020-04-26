using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Scheduler.Jobs.Interface
{
    public interface IPlugInJob
    {
        void Exec();

        EnumJobs Name();
        
    }

    public enum EnumJobs
    {
        UpdateWeb,
        UpdateFromWeb,
        BackupDB

    }
}
