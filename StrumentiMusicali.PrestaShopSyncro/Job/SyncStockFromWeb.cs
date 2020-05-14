using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using StrumentiMusicali.PrestaShopSyncro.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.PrestaShopSyncro.Job
{
    public class SyncStockFromWeb : IPlugInJob
    {
        public void Exec()
        {
            using (var orderSync=new OrderSync())
            {
                orderSync.UpdateFromWeb();
            }
        }

        public EnumJobs Name()
        {
            return EnumJobs.UpdateFromWeb;
        }
    }
}
