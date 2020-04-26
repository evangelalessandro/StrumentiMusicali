using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro.Products
{
    internal class StockProducts : BaseProduct
    {
        /// <summary>
        /// Aggiorna il magazzino del articolo nel web
        /// </summary>
        /// <param name="newArtWeb"></param>
        /// <param name="artDb"></param>
        public void UpdateStockArt(product newArtWeb, ArticoloBase artDb, UnitOfWork uof, bool forzaUpdate=false)
        {
            var stock = _StockAvailableFactory.Get(newArtWeb.associations.stock_availables.First().id);
            if (!forzaUpdate && artDb.Aggiornamento.GiacenzaMagazzinoWebInDataAggWeb != stock.quantity)
            {
                Core.Manager.ManagerLog.Logger.Info("Il prodotto '" + artDb.ArticoloDb.Titolo +
                    "' è stato modificato con gli ordini nel web, occorre prima aspettare che venga " +
                    "scaricata la nuova qta dal job delle giacenze.");
                return;
            }
            DateTime date = DateTime.Now;
            stock.quantity = CalcolaStock(artDb);
            _StockAvailableFactory.Update(stock);
            artDb.Aggiornamento.DataUltimoAggMagazzino = date;
            artDb.Aggiornamento.DataUltimoAggMagazzinoWeb = date;
            artDb.Aggiornamento.GiacenzaMagazzinoWebInDataAggWeb = stock.quantity;
            SalvaAggiornamento(uof, artDb.Aggiornamento);
        }

        public List<ArticoloBase> UpdateStock(UnitOfWork uof)
        {
            var aggiornamentoWebs = uof.AggiornamentoWebArticoloRepository.Find(a => (a.Articolo.CaricainECommerce

                               && a.Articolo.Categoria.Codice >= 0
                              && a.Articolo.ArticoloWeb.PrezzoWeb > 0
                              )
                              || a.ForzaAggiornamento==true).
                              Select(a => new ArticoloBase
                              {
                                  CodiceArticoloEcommerce =
                                    a.CodiceArticoloEcommerce,
                                  ArticoloID = a.Articolo.ID,
                                  Aggiornamento = a,
                                  ArticoloDb = a.Articolo
                              }).ToList()
                                .Where(a => Math.Abs((a.Aggiornamento.DataUltimoAggMagazzino - a.Aggiornamento.DataUltimoAggMagazzinoWeb)
                                .TotalSeconds) > 10
                                || a.Aggiornamento.ForzaAggiornamento == true)
                                .ToList();
            foreach (var item in aggiornamentoWebs)
            {
                UpdateStockArt(item, uof);
            }
            return aggiornamentoWebs;
        }

        public int CalcolaStock(ArticoloBase artDb)
        {
            using (var uof = new UnitOfWork())
            {
                var giacenza = uof.MagazzinoRepository.Find(a => artDb.ArticoloID.Equals(a.ArticoloID))
                               .Select(a => new { a.ArticoloID, a.Qta, a.Deposito }).GroupBy(a => new { a.ArticoloID, a.Deposito })
                               .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key }).ToList();

                //var val = giacenza.Where(a => a.Articolo.ArticoloID == item.ID).ToList();
                //.Select(a => a.Sum).FirstOrDefault();

                return giacenza.Where(a => a.Articolo.Deposito.Principale).Select(a => a.Sum)
                        .DefaultIfEmpty(0).FirstOrDefault();

                //item.QuantitaTotale = val.Sum(a => a.Sum);
            }
        }

        private void UpdateStockArt(ArticoloBase artDb, UnitOfWork uof)
        {
            if (!string.IsNullOrEmpty(artDb.CodiceArticoloEcommerce))
            {
                product artWeb = GetProdWebFromCodartEcommerce(artDb);

                UpdateStockArt(artWeb, artDb, uof);
            }
        }
    }
}