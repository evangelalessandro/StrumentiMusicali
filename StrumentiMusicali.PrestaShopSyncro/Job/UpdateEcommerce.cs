using StrumentiMusicali.Core.Scheduler.Jobs.Interface;

namespace StrumentiMusicali.PrestaShopSyncro.Job
{
    public class UpdateEcommerce : IPlugInJob
    {
        private static bool _execting = false;

        public void Exec()
        {
            try
            {
                if (_execting)
                    return;
                _execting = true;
                using (var prod = new ProductSyncro())
                {
                    prod.AggiornaWeb();
                }
            }
            finally
            {
                _execting = false;
            }
        }

        public EnumJobs Name()
        {
            return EnumJobs.UpdateWeb;
        }
    }
}