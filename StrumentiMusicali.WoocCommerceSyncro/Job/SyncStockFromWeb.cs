using StrumentiMusicali.Core.Scheduler.Jobs.Interface;
using StrumentiMusicali.WooCommerceSyncro.Sync;

namespace StrumentiMusicali.WooCommerceSyncro.Job
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
            return EnumJobs.WooCommerceUpdateFromWeb;
        }
    }
}
