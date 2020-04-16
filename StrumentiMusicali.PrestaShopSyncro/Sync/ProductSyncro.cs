using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.PrestaShopSyncro.Products;
using System;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro
{
    public class ProductSyncro : BaseProduct
    {
        /// <summary>
        /// Tutti gli articoli che sono con il flag CaricainECommerce e
        /// </summary>
        public void AggiornaWeb()
        {
            using (var uof = new UnitOfWork())
            {
                using (var groupsync = new CategorySync())
                {
                    groupsync.AllineaCategorieReparti();
                }
                UpdateProducts(uof);

                using (var stockPr = new StockProducts())
                {
                    stockPr.UpdateStock(uof);
                }
                using (var imgPr = new ImageProduct())
                {
                    imgPr.UpdateImages(uof);
                }
            }
        }

        private void UpdateProducts(UnitOfWork uof)
        {
            DateTime dataLettura = DateTime.Now;
            var listaArt = uof.AggiornamentoWebArticoloRepository.Find(a => a.Articolo.CaricainECommerce
                && (a.DataUltimoAggiornamentoWeb < a.Articolo.DataUltimaModifica

              ) && a.Articolo.Categoria.Codice >= 0
              && a.Articolo.ArticoloWeb.PrezzoWeb > 0).Select(a => new ArticoloBase
              {
                  ArticoloDb = a.Articolo,
                  Aggiornamento = a,
                  ArticoloID = a.Articolo.ID,
                  CodiceArticoloEcommerce = a.CodiceArticoloEcommerce
              }).ToList()
              .Where(a => Math.Abs((a.Aggiornamento.DataUltimoAggiornamentoWeb - a.ArticoloDb.DataUltimaModifica).TotalSeconds) > 10)
              .ToList();
            foreach (var item in listaArt.Take(10))
            {
                UpdateProduct(item, dataLettura);
            }
        }

        private void UpdateProduct(ArticoloBase artDb, DateTime dataLettura)
        {
            using (var uof = new UnitOfWork())
            {
                product artWeb = new product();
                if (!string.IsNullOrEmpty(artDb.Aggiornamento.CodiceArticoloEcommerce))
                {
                    artWeb = _productFactory.Get(long.Parse(artDb.Aggiornamento.CodiceArticoloEcommerce));
                }
                ManagerLog.Logger.Info("Caricamento in corso dell'articolo '" + artDb.ArticoloDb.Titolo + "' ID=" + artDb.ArticoloID + "  nel web");
                setDataItemWeb(artDb, uof, artWeb);

                try
                {
                    if (!artWeb.id.HasValue)
                    {
                        artWeb = _productFactory.Add(artWeb);
                    }
                    else
                    {
                        _productFactory.Update(artWeb);
                    }
                    artDb.Aggiornamento.DataUltimoAggiornamentoWeb = dataLettura;
                    artDb.Aggiornamento.CodiceArticoloEcommerce = artWeb.id.Value.ToString();
                    uof.AggiornamentoWebArticoloRepository.Update(artDb.Aggiornamento);
                    uof.Commit();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void setDataItemWeb(ArticoloBase artDb, UnitOfWork uof, product artWeb)
        {
            artWeb.price = Math.Round(artDb.ArticoloDb.ArticoloWeb.PrezzoWeb * 100 / (100 + artDb.ArticoloDb.ArticoloWeb.Iva), 6, MidpointRounding.ToEven);
            artWeb.name.Clear();
            artWeb.AddName(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.ArticoloDb.Titolo));
            artWeb.link_rewrite.Clear();
            artWeb.AddLinkRewrite(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.ArticoloDb.Titolo));
            artWeb.active = 1;
            artWeb.condition = "new";
            artWeb.show_condition = 1;
            //*iva al 22%*//
            artWeb.id_tax_rules_group = 1;
            switch (artDb.ArticoloDb.Condizione)
            {
                case enCondizioneArticolo.Nuovo:
                    artWeb.condition = "new";
                    break;

                case enCondizioneArticolo.ExDemo:
                    artWeb.condition = "exdemo";
                    break;

                case enCondizioneArticolo.UsatoGarantito:
                    artWeb.condition = "used";
                    break;

                case enCondizioneArticolo.NonSpecificato:
                    break;

                default:
                    break;
            }
            artWeb.show_price = 1;

            artWeb.id_category_default = uof.CategorieWebRepository.Find(a => a.CategoriaID == artDb.ArticoloDb.CategoriaID).FirstOrDefault().CodiceWeb;

            var categDb = uof.CategorieRepository.Find(a => a.ID == artDb.ArticoloDb.CategoriaID).First();

            var listaCateg = uof.CategorieWebRepository.Find(a => a.Categoria.Nome == categDb.Nome).Select(a => a.CodiceWeb).ToList();

            artWeb.associations.categories.Clear();
            foreach (var item in listaCateg)
            {
                artWeb.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(item));
            }

            if (string.IsNullOrEmpty(artDb.ArticoloDb.ArticoloWeb.DescrizioneHtml))
                artDb.ArticoloDb.ArticoloWeb.DescrizioneHtml = "";
            artWeb.description.Clear();
            artWeb.description.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1,
            artDb.ArticoloDb.ArticoloWeb.DescrizioneHtml));

            if (string.IsNullOrEmpty(artDb.ArticoloDb.ArticoloWeb.DescrizioneBreveHtml))
                artDb.ArticoloDb.ArticoloWeb.DescrizioneBreveHtml = "";
            artWeb.description_short.Clear();
            artWeb.description_short.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1,
                artDb.ArticoloDb.ArticoloWeb.DescrizioneBreveHtml));
            artWeb.available_for_order = 1;
            artWeb.New = 0;
            artWeb.state = 1;
            artWeb.visibility = "both";
            artWeb.minimal_quantity = 1;
            artWeb.reference = artDb.ArticoloID.ToString();
        }

        /*leggo i prodotti che sono da aggiornare */
    }
}