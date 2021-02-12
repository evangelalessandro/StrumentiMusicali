
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.WooCommerceSyncro.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.WooCommerceSyncro.Products
{
    internal class StockProducts : StockProductsBase
    {
        SyncroBaseWooCommerce _syncroBaseWooCommerce;
        public StockProducts(SyncroBaseWooCommerce syncroBaseWooCommerce)
        {
            _syncroBaseWooCommerce = syncroBaseWooCommerce;
        }


        /// <summary>
        /// Aggiorna il magazzino del articolo nel web
        /// </summary>
        /// <param name="newArtWeb"></param>
        /// <param name="artDb"></param>
        public bool UpdateStockArt(WooCommerceNET.WooCommerce.v3.Product newArtWeb, ArticoloBase artDb, UnitOfWork uof, bool forzaUpdate = false)
        {

            var qta = newArtWeb.stock_quantity.Value;
            
            var updated=UpdateStockArt(ref qta, artDb, uof, forzaUpdate);

            if (updated && newArtWeb.stock_quantity != qta)
            { 
                newArtWeb.stock_quantity = qta;

                _syncroBaseWooCommerce.UpdateProdWeb( newArtWeb);
                return true;
            }
            return false;
        }

        public override bool UpdateStockArt(ArticoloBase artDb, UnitOfWork uof)
        {

            if (!string.IsNullOrEmpty(artDb.CodiceArticoloEcommerce))
            {
                var boolNew = false;
                var artWeb = _syncroBaseWooCommerce.GetProdWeb(artDb, out boolNew);

                return UpdateStockArt(artWeb, artDb, uof);
            }
            return false;
        }
    }
}