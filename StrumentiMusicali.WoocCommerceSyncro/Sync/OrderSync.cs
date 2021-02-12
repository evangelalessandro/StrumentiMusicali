
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.WooCommerceSyncro.Sync
{
    public class OrderSync : BaseClass.SyncroBaseWooCommerce
    {
        public void UpdateFromWeb(bool onlyOne = false)
        {
            var _orderFact = _wc.Order;

            var setting = SettingSitoValidator.ReadSetting();

            DateTime StartDate = setting.WooCommerceSetting.DataUltimoOrdineGiacenza;
            //if (StartDate.Year == 1900)
            //{
            //    Core.Manager.ManagerLog.Logger.Warn(@"Occorre impostare la data ultimo ordine
            //        giacenza nei settaggi per il sito prestashop");
            //    return;
            //}
            if (StartDate.Year==1900)
            {
                StartDate = DateTime.Now.AddDays(-45);
            }
            Dictionary<string, string> filter = new Dictionary<string, string>();
            string dFrom = StartDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            filter.Add("after", dFrom);
            filter.Add("orderby", "id");

            var ordersT = _orderFact.GetAll(filter);
            ordersT.Wait();
            var orders = ordersT.Result;
            if (orders.Count() == 0)
            {
                /*Non ci sono aggiornamenti*/
                return;
            }
            var maxUpdate = orders.Select(a => a.date_created.Value).ToList()
                .Max(a => a);

            var righe = orders.SelectMany(a => a.line_items)
                .Where(a => a.product_id.HasValue).Select(a => a.product_id.Value).Distinct().ToList();

            using (var uof = new UnitOfWork())
            {
                var depoPrinc = uof.DepositoRepository.Find(a => a.Principale == true).First();

                UpdateProdotti(righe, uof, depoPrinc);
                /*rileggo il dato per essere sicuro che sia atomica la transazione*/
                setting = SettingSitoValidator.ReadSetting();
                setting.WooCommerceSetting.DataUltimoOrdineGiacenza = maxUpdate.AddSeconds(1);
                uof.SettingSitoRepository.Update(setting);
                uof.Commit();
            }
        }

        private void UpdateProdotti(List<int> prodotti, UnitOfWork uof, Deposito depoPrinc)
        {
            foreach (var idProdotto in prodotti)
            {
                var prodottoTsk = _wc.Product.Get(idProdotto);
                prodottoTsk.Wait();
                var prodotto = prodottoTsk.Result;

                var aggiornamento = uof.AggiornamentoWebArticoloRepository.
                    Find(a => a.CodiceArticoloEcommerce == idProdotto.ToString()).FirstOrDefault();
                /*è nullo solo se l'articolo è nel web ma non in locale*/
                if (aggiornamento == null)
                    continue;
                if (aggiornamento.GiacenzaMagazzinoWebInDataAggWeb != prodotto.stock_quantity)
                {

                    var qtaStockLocale = StockProductsBase.CalcolaStock(new ArticoloBase() { ArticoloID = aggiornamento.ArticoloID });
                    var forzaUpdateGiacenza = (aggiornamento.GiacenzaMagazzinoWebInDataAggWeb != qtaStockLocale);


                    var dataAgg = DateTime.Now;
                    ///se c'è stata una vendita allora aggiungo un movimento di magazzino
                    uof.MagazzinoRepository.Add(new Library.Entity.Magazzino()
                    {
                        ArticoloID = aggiornamento.ArticoloID,
                        DepositoID = depoPrinc.ID,
                        Qta = -(aggiornamento.GiacenzaMagazzinoWebInDataAggWeb - prodotto.stock_quantity.Value),
                        PrezzoAcquisto = 0,
                        Note = aggiornamento.GiacenzaMagazzinoWebInDataAggWeb > prodotto.stock_quantity.Value ? "Vendita web Annullata" : "Vendita web Annullata",
                        OperazioneWeb = true
                    });

                    aggiornamento.GiacenzaMagazzinoWebInDataAggWeb = prodotto.stock_quantity.Value;
                    aggiornamento.DataUltimoAggMagazzinoWeb = dataAgg;
                    aggiornamento.DataUltimoAggMagazzino = dataAgg;
                    uof.AggiornamentoWebArticoloRepository.Update(aggiornamento);
                    uof.Commit();

                    if (forzaUpdateGiacenza)
                    {

                        var qta = prodotto.stock_quantity.Value;
                        StockProductsBase.UpdateStockArt(ref qta, new ArticoloBase()
                        {
                            ArticoloID = aggiornamento.ArticoloID,
                            Aggiornamento = aggiornamento,
                            CodiceArticoloEcommerce = aggiornamento.CodiceArticoloEcommerce,
                            ArticoloDb = aggiornamento.Articolo
                        }, uof, true);

                    }
                }
            }
        }
    }
}