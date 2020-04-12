using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro
{
    public class ProductSyncro : BaseClass.SyncroBase
    {
        /// <summary>
        /// Tutti gli articoli che sono con il flag CaricainECommerce e
        /// </summary>
        public void AggiornaWeb()
        {
            using (var uof = new UnitOfWork())
            {
                var groupsync = new CategorySync();
                groupsync.AllineaCategorieReparti();
                DateTime dataLettura = DateTime.Now;
                var listaArt = uof.ArticoliRepository.Find(a => a.CaricainECommerce
                    && (a.ArticoloWeb.DataUltimoAggiornamentoWeb < a.DataUltimaModifica
                  || a.ImmaginiDaCaricare) && a.Categoria.Codice >= 0
                  && a.ArticoloWeb.PrezzoWeb > 0);
                foreach (var item in listaArt)
                {
                    CaricaArticolo(item, dataLettura);
                    return;
                }
            }
        }


       
        private int CalcolaStock(Articolo artDb)
        {
            using (var uof = new UnitOfWork())
            {
                var giacenza = uof.MagazzinoRepository.Find(a => artDb.ID.Equals(a.ArticoloID))
                               .Select(a => new { a.ArticoloID, a.Qta, a.Deposito }).GroupBy(a => new { a.ArticoloID, a.Deposito })
                               .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key }).ToList();

                //var val = giacenza.Where(a => a.Articolo.ArticoloID == item.ID).ToList();
                //.Select(a => a.Sum).FirstOrDefault();

                return giacenza.Where(a => a.Articolo.Deposito.Principale).Select(a => a.Sum)
                        .DefaultIfEmpty(0).FirstOrDefault();

                //item.QuantitaTotale = val.Sum(a => a.Sum);
            }
        }

        private void CaricaArticolo(Articolo artDb, DateTime dataLettura)
        {
            using (var uof = new UnitOfWork())
            {
                product artWeb = new product();
                artWeb.price = Math.Round(artDb.ArticoloWeb.PrezzoWeb * 100 / (100 + artDb.ArticoloWeb.Iva), 6, MidpointRounding.ToEven);
                ManagerLog.AddLogMessage("Caricamento in corso dell'articolo '" + artDb.Titolo + "' ID=" + artDb.ID + "  nel web");

                artWeb.AddName(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.Titolo));
                artWeb.AddLinkRewrite(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.Titolo));
                artWeb.active = 1;
                artWeb.condition = "new";
                artWeb.show_condition = 1;
                //*iva al 22%*//
                artWeb.id_tax_rules_group = 1;
                switch (artDb.Condizione)
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

                artWeb.id_category_default = uof.CategorieWebRepository.Find(a => a.CategoriaID == artDb.CategoriaID).FirstOrDefault().CodiceWeb;

                var categDb = uof.CategorieRepository.Find(a => a.ID == artDb.CategoriaID).First();

                var listaCateg = uof.CategorieWebRepository.Find(a => a.Categoria.Nome == categDb.Nome).Select(a => a.CodiceWeb).ToList();

                foreach (var item in listaCateg)
                {
                    artWeb.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(item));
                }


                if (string.IsNullOrEmpty(artDb.ArticoloWeb.DescrizioneHtml))
                    artDb.ArticoloWeb.DescrizioneHtml = "";
                artWeb.description.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1,
                artDb.ArticoloWeb.DescrizioneHtml));

                if (string.IsNullOrEmpty(artDb.ArticoloWeb.DescrizioneBreveHtml))
                    artDb.ArticoloWeb.DescrizioneBreveHtml = "";
                artWeb.description_short.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1,
                    artDb.ArticoloWeb.DescrizioneBreveHtml));
                artWeb.available_for_order = 1;
                artWeb.New = 0;
                artWeb.state = 1;
                artWeb.visibility = "both";
                artWeb.minimal_quantity = 1;
                artWeb.reference = artDb.ID.ToString();

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

                    artDb.ArticoloWeb.DataUltimoAggiornamentoWeb = dataLettura;
                    artDb.ArticoloWeb.CodiceArticoloEcommerce = artWeb.id.Value.ToString();

                    UploadImage(artDb, artWeb, uof);

                    UpdateStockArt(artWeb, artDb);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void UploadImage(Articolo artDb, product artWeb, UnitOfWork uof)
        {
            /*cancello le immagini e le sovrascrivo*/
            foreach (var item in artWeb.associations.images)
            {
                _imageFactory.DeleteProductImage(artWeb.id.Value, item.id);
            }
            var settingSito = SettingSitoValidator.ReadSetting();

            var imageList = uof.FotoArticoloRepository.Find(a => a.ArticoloID == artDb.ID).OrderBy(a => a.Ordine).ToList();

            var listFotoArticolo = imageList.Select(a => new ImmaginiFile<FotoArticolo>(Path.Combine(
                settingSito.CartellaLocaleImmagini, a.UrlFoto)
                             , a.UrlFoto, a)).ToList();

            foreach (var item in listFotoArticolo)
            {
                _imageFactory.AddProductImage(artWeb.id.Value, item.File);
            }

        }

        

        /// <summary>
        /// Aggiorna il magazzino del articolo nel web
        /// </summary>
        /// <param name="newArtWeb"></param>
        /// <param name="artDb"></param>
        private void UpdateStockArt(product newArtWeb, Articolo artDb)
        {
            var stock = _StockAvailableFactory.Get(newArtWeb.associations.stock_availables.First().id);

            stock.quantity = CalcolaStock(artDb);
            _StockAvailableFactory.Update(stock);
        }

        /*leggo i prodotti che sono da aggiornare */

        
    }
}