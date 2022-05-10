using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.PrestaShopSyncro.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro.Products
{
    internal class StockProducts : StockProductsBase, IDisposable
    {
        SyncroBasePresta _syncroBasePresta;
        public StockProducts(SyncroBasePresta syncroBasePresta)
        {
            _syncroBasePresta = syncroBasePresta;
        }

        public void Dispose()
        {
            if (_syncroBasePresta != null)
                _syncroBasePresta.Dispose();

        }

        /// <summary>
        /// Aggiorna il magazzino del articolo nel web
        /// </summary>
        /// <param name="newArtWeb"></param>
        /// <param name="artDb"></param>
        public bool UpdateStockArt(product newArtWeb, ArticoloBase artDb, UnitOfWork uof, bool forzaUpdate = false)
        {

            var stock = _syncroBasePresta._StockAvailableFactory.Get(newArtWeb.associations.stock_availables.First().id);
            var qta = stock.quantity;
            var updated =UpdateStockArt(ref qta, artDb, uof, forzaUpdate);
            if (updated && stock.quantity != qta && Environment.MachineName!="EVANGALE")
            {
				_logger.Info("Ecomm: Prod_id:" + newArtWeb.id + " name: " + artDb.ArticoloDb.Titolo + "| giacenza vecchia:" + stock.quantity.ToString() + " nuova giacenze : " + qta.ToString());

				stock.quantity = qta;
				
                _syncroBasePresta._StockAvailableFactory.Update(stock);
                return true;
            }
            return false;
        }

        public override bool UpdateStockArt(ArticoloBase artDb, UnitOfWork uof)
        {

            if ((artDb.CodiceArticoloEcommerce>0))
            {
                product artWeb = _syncroBasePresta.GetProdWebFromCodartEcommerce(artDb);

                return UpdateStockArt(artWeb, artDb, uof);

            }
            return false;
        }
    }
}