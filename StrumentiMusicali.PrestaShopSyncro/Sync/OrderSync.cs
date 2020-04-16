using Bukimedia.PrestaSharp.Factories;
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro
{
    public class OrderSync : BaseClass.SyncroBase
    {
        public void UpdateFromWeb()
        {
            var _orderFact = new OrderFactory(_url, _autKey, "");

            var setting = SettingSitoValidator.ReadSetting();

            DateTime StartDate = setting.PrestaShopSetting.DataUltimoOrdineGiacenza;
            DateTime EndDate = DateTime.Now.AddDays(1);
            Dictionary<string, string> filter = new Dictionary<string, string>();
            string dFrom = string.Format("{0:yyyy-MM-dd HH:mm:ss}", StartDate);
            string dTo = string.Format("{0:yyyy-MM-dd HH:mm:ss}", EndDate);
            filter.Add("date_upd", "[" + dFrom + "," + dTo + "]");

            var orders = _orderFact.GetByFilter(filter, "date_upd_ASC", null);

            if (orders.Count() == 0)
            {
                /*Non ci sono aggiornamenti*/
                return;
            }
            var maxUpdate = orders.Select(a => a.date_upd).ToList()
                .Select(a => DateTime.Parse(a)).Max(a => a);

            var righe = orders.SelectMany(a => a.associations.order_rows)
                .Where(a => a.product_id.HasValue).Select(a => a.product_id.Value).Distinct().ToList();

            using (var uof = new UnitOfWork())
            {
                var depoPrinc = uof.DepositoRepository.Find(a => a.Principale == true).First();

                UpdateProdotti(righe, uof, depoPrinc);
                /*rileggo il dato per essere sicuro che sia atomica la transazione*/
                setting = SettingSitoValidator.ReadSetting();
                setting.PrestaShopSetting.DataUltimoOrdineGiacenza = maxUpdate.AddSeconds(1);
                uof.SettingSitoRepository.Update(setting);
                uof.Commit();
            }
        }

        private void UpdateProdotti(List<long> righe, UnitOfWork uof, Deposito depoPrinc)
        {
            foreach (var idProdotto in righe)
            {
                var prodotto = _productFactory.Get(idProdotto);

                var stock = _StockAvailableFactory.Get(prodotto.associations.stock_availables.First().id);
                var aggiornamento = uof.AggiornamentoWebArticoloRepository.
                    Find(a => a.CodiceArticoloEcommerce == idProdotto.ToString()).FirstOrDefault();
                /*è nullo solo se l'articolo è nel web ma non in locale*/
                if (aggiornamento == null)
                    continue;
                if (aggiornamento.GiacenzaMagazzinoWebInDataAggWeb != stock.quantity)
                {
                    var dataAgg = DateTime.Now;
                    ///se c'è stata una vendita allora aggiungo un movimento di magazzino
                    uof.MagazzinoRepository.Add(new Library.Entity.Magazzino()
                    {
                        ArticoloID = aggiornamento.ArticoloID,
                        DepositoID = depoPrinc.ID,
                        Qta = -(aggiornamento.GiacenzaMagazzinoWebInDataAggWeb - stock.quantity),
                        PrezzoAcquisto = 0,
                        Note = aggiornamento.GiacenzaMagazzinoWebInDataAggWeb > stock.quantity ? "Vendita web Annullata" : "Vendita web Annullata",
                        OperazioneWeb = true
                    });
 
                    aggiornamento.GiacenzaMagazzinoWebInDataAggWeb = stock.quantity;
                    aggiornamento.DataUltimoAggMagazzinoWeb = dataAgg;
                    aggiornamento.DataUltimoAggMagazzino = dataAgg;
                    uof.AggiornamentoWebArticoloRepository.Update(aggiornamento);
                    uof.Commit();
                }
            }
        }
    }
}