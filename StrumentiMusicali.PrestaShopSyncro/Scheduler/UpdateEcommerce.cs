using StrumentiMusicali.Core.Scheduler.Jobs.Interface;

namespace StrumentiMusicali.PrestaShopSyncro.Scheduler
{
    public class UpdateEcommerce : IUpdateEcommerce
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
    }
}