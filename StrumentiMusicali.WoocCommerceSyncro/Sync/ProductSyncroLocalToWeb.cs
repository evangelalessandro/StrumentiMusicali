
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Entity.Enums;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.WooCommerceSyncro.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using WooCommerceNET.WooCommerce.v3;

namespace StrumentiMusicali.WooCommerceSyncro.Sync
{
    public class ProductSyncroLocalToWeb : BaseClass.SyncroBaseWooCommerce
    {
        /// <summary>
        /// Tutti gli articoli che sono con il flag CaricainECommerce e
        /// </summary>
        public void AggiornaWeb()
        {

            using (var groupsync = new CategorySync())
            {

                //groupsync.AllineaCategorieReparti();
            }
            var listArt = UpdateProducts();
            using (var uof = new UnitOfWork())
            {
                var stockPr = new StockProducts(this);
                {
                    var listStockArt = stockPr.UpdateStock(uof);

                    listArt.AddRange(listStockArt);
                }
                //using (var imgPr = new ImageProduct())
                //{
                //    listArt.AddRange(imgPr.UpdateImages(uof));
                //}
                var listArtId = listArt.FindAll(a => a.Aggiornamento.ForzaAggiornamento == true).Select(a => a.ArticoloID).Distinct().ToList();

                var aggToFix = uof.AggiornamentoWebArticoloRepository.Find(a => listArtId.Contains(a.ArticoloID) && a.ForzaAggiornamento).ToList();
                foreach (var item in aggToFix)
                {
                    uof.AggiornamentoWebArticoloRepository.Update(item);
                }
                uof.Commit();
            }
        }

        private List<ArticoloBase> UpdateProducts( )
        {
           
            using (var uof = new UnitOfWork())
            {
                
                DateTime dataLettura = DateTime.Now;
                var listaArt = base.ListArt(uof, false).Take(10).ToList();

                foreach (var item in listaArt)
                {
                    UpdateProduct(item, dataLettura);
                }
                return listaArt;
            }
        }

        private void UpdateProduct(ArticoloBase artDb, DateTime dataLettura)
        {
            using (var uof = new UnitOfWork())
            {
                
                bool newProd;
                
                var artWeb=GetProdWeb(artDb,  out newProd);

                ManagerLog.Logger.Info("Caricamento in corso dell'articolo '" + artDb.ArticoloDb.Titolo + "' ID=" + artDb.ArticoloID + "  nel web");
                SetDataItemWeb(artDb, uof, artWeb, newProd);

                try
                {
                    artWeb = UpdateProdWeb(artWeb);
                    artDb.Aggiornamento.DataUltimoAggiornamentoWeb = dataLettura; 
                    artDb.Aggiornamento.CodiceArticoloEcommerce = artWeb.id.Value.ToString();
                    artDb.Aggiornamento.Link = artWeb.permalink;

                    uof.AggiornamentoWebArticoloRepository.Update(artDb.Aggiornamento);
                    uof.Commit();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

      

        private void SetDataItemWeb(ArticoloBase artDb, UnitOfWork uof, WooCommerceNET.WooCommerce.v3.Product artWeb, bool newProd)
        {
            artWeb.regular_price = Math.Round(artDb.ArticoloDb.ArticoloWeb.PrezzoWeb * 100 / (100 + artDb.ArticoloDb.ArticoloWeb.Iva), 6, MidpointRounding.ToEven);
            //artWeb.sale_price = Math.Round(artDb.ArticoloDb.ArticoloWeb.PrezzoWeb * 100 / (100 + artDb.ArticoloDb.ArticoloWeb.Iva), 6, MidpointRounding.ToEven);
            artWeb.name = artDb.ArticoloDb.Titolo;


            //*iva al 22%*//
            artWeb.tax_class = artDb.ArticoloDb.ArticoloWeb.Iva.ToString();
            //switch (artDb.ArticoloDb.Condizione)
            //{
            //    case enCondizioneArticolo.Nuovo:
            //        artWeb.condition = "new";
            //        break;

            //    case enCondizioneArticolo.ExDemo:
            //        artWeb.condition = "used"; //refurbished
            //        break;

            //    case enCondizioneArticolo.UsatoGarantito:
            //        artWeb.condition = "used";
            //        break;

            //    case enCondizioneArticolo.NonSpecificato:
            //        break;

            //    default:
            //        break;
            //} 
            string reparto = "";
            artWeb.categories = new List<WooCommerceNET.WooCommerce.v3.ProductCategoryLine>();
            artWeb.categories.Clear();

            if (
                artDb.ArticoloDb.Condizione == enCondizioneArticolo.ExDemo
                    || artDb.ArticoloDb.Condizione == enCondizioneArticolo.UsatoGarantito)
            {

                if (artDb.ArticoloDb.Condizione == enCondizioneArticolo.ExDemo)
                {
                    reparto = CategorySync.ExDemo;
                }
                else if (artDb.ArticoloDb.Condizione == enCondizioneArticolo.UsatoGarantito)
                {
                    reparto = CategorySync.Usato;
                }
                artWeb.categories.Add(new WooCommerceNET.WooCommerce.v3.ProductCategoryLine()
                {
                    id = (int)uof.RepartoWebRepository.Find(a =>
                        a.Reparto == reparto).FirstOrDefault().CodiceWeb
                });  



            }
            else
            {

                var categDb = uof.CategorieRepository.Find(a => a.ID == artDb.ArticoloDb.CategoriaID).First();

                var listaCateg = uof.CategorieWebRepository.Find(a => a.Categoria.Nome == categDb.Nome).Select(a => a.CodiceWeb).ToList();
                foreach (var item in listaCateg)
                {
                    artWeb.categories.Add(new WooCommerceNET.WooCommerce.v3.ProductCategoryLine() { id = (int)item });
                }
            }
            if (string.IsNullOrEmpty(artDb.ArticoloDb.ArticoloWeb.DescrizioneHtml))
            {
                artWeb.description = "";
                artWeb.enable_html_description = false;
            }
            else
            {
                artWeb.description = artDb.ArticoloDb.ArticoloWeb.DescrizioneHtml;
                artWeb.enable_html_description = true;
            } 
            artWeb.short_description =
                artDb.ArticoloDb.ArticoloWeb.DescrizioneBreveHtml;
            artWeb.enable_html_short_description = "true";


            if (artDb.ArticoloDb.Condizione!=enCondizioneArticolo.Nuovo)
            {
                artWeb.name = "USATO " + artWeb.name;
                artWeb.description = "USATO " + artWeb.description;
            }
          
            artWeb.manage_stock = true;
            
            if (newProd)
                artWeb.stock_quantity = StockProductsBase.CalcolaStock(new ArticoloBase() { ArticoloID = artDb.ArticoloID}); 


            artWeb.sku = artDb.ArticoloID.ToString();


        }

    }
}