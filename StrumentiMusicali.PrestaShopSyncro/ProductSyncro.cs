using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;

using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Core.Manager;

namespace StrumentiMusicali.PrestaShopSyncro
{
    public class ProductSyncro
    {

        string _url = "";
        string _autKey = "";
        public ProductSyncro()
        {
            var logondata = LoginData();
            _url = logondata.WebServiceUrl;
            _url += "api/";
            _autKey = logondata.AuthKey;
            _autKey = "I7APN3Z45A5YYN6R24GLSK3X3JIG24GI";
            _url = "http://localhost:10280/prestashop/api/";
            _imageFactory = new ImageFactory(_url, _autKey, "");
            _StockAvailableFactory = new StockAvailableFactory(_url, _autKey, "");
            _productFactory = new ProductFactory(_url, _autKey, "");
            _categoriesFact = new CategoryFactory(_url, _autKey, "");
        }
        CategoryFactory _categoriesFact = null;
        StockAvailableFactory _StockAvailableFactory = null;
        ImageFactory _imageFactory = null;
        ProductFactory _productFactory = null;

        /// <summary>
        /// Tutti gli articoli che sono con il flag CaricainECommerce e 
        /// </summary>
        public void AggiornaWeb()
        {
            using (var uof = new UnitOfWork())
            {
                AllineaCategorieReparti();
                var listaArt = uof.ArticoliRepository.Find(a => a.CaricainECommerce
                    && (a.ArticoloWeb.DataUltimoAggiornamentoWeb < a.DataUltimaModifica
                  || a.ImmaginiDaCaricare) && a.Categoria.Codice >= 0);
                foreach (var item in listaArt)
                {
                    CaricaArticolo(item);
                    return;
                }

            }
        }

        public void AllineaCategorieReparti()
        {
            ManagerLog.AddLogMessage("Avviato allineamento categorie reparti");
            List<category> listCateg = AllineaReparti();
            AllineaCategorie(listCateg);
            ManagerLog.AddLogMessage("Terminato allineamento categorie reparti");
        }

        private void AllineaCategorie(List<category> listCategories)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                long rootCateg = listCategories.Where(a => a.is_root_category == 1).First().id.Value;

                //legge da db le categorie e i reparti
                foreach (var categDb in uof.CategorieRepository.Find(a => a.Codice > 0)
                    .Select(a => new { a.Reparto, NomeCateg = a.Nome, a.ID })
                    .OrderBy(a => a.Reparto).OrderBy(a => a.NomeCateg).ToList())
                {
                    var categRepartoWebId = uof.RepartoWebRepository.Find(a => 1 == 1).Where(a => a.Reparto
                    == categDb.Reparto).FirstOrDefault().CodiceWeb;

                    /*verifico che esista il reparto tra le categorie web */
                    var repartoWeb = listCategories.Where(a => a.name.First().Value == categDb.Reparto && a.id_parent == rootCateg).DefaultIfEmpty(null).FirstOrDefault();

                    if (repartoWeb != null)
                    {
                        /*controllo che non esista già la categoria dentro al reparto nel ecomm*/
                        var existsCateg = listCategories.Where(a => a.id_parent == repartoWeb.id)
                            .Where(a => a.name.First().Value == categDb.NomeCateg).FirstOrDefault();
                        if (existsCateg == null)
                        {
                            var newCateg = AggiungiCategoriaWeb(categRepartoWebId, categDb.NomeCateg);

                            uof.CategorieWebRepository.Add(new Library.Entity.Ecomm.CategoriaWeb()
                            { CategoriaID = categDb.ID, CodiceWeb = newCateg.id.Value });
                            uof.Commit();

                            StrumentiMusicali.Core.Manager.ManagerLog.AddLogMessage("Aggiunta categoria web: " + categDb.NomeCateg);
                        }
                        else
                        {
                            /*controlla che il codice web sia lo stesso*/
                            var categWebDb = uof.CategorieWebRepository.Find(a => a.CategoriaID == categDb.ID).FirstOrDefault();
                            if (categWebDb == null)
                            {
                                uof.CategorieWebRepository.Add(new Library.Entity.Ecomm.CategoriaWeb()
                                { CategoriaID = categDb.ID, CodiceWeb = existsCateg.id.Value });
                                uof.Commit();
                            }
                            else
                            {
                                categWebDb.CodiceWeb = existsCateg.id.Value;
                                uof.CategorieWebRepository.Update(categWebDb);
                                uof.Commit();
                            }
                        }

                    }
                    else
                    {
                        var ex = new MessageException("Manca il reparto " + categDb.Reparto + "  nel ecommerce");
                        StrumentiMusicali.Core.Manager.ManagerLog.AddLogException(ex.Message, ex);

                    }
                }
            }
        }
        /// <summary>
        /// Aggiorna le categorie mettendo i reparti al livello superiore
        /// </summary>
        /// <returns>ritorna le categorie lette dal web</returns>
        private List<category> AllineaReparti()
        {
            bool retVal = false;
            List<category> listCategories = LeggiCategorieDalWeb();
            /*prima metto i reparti sotto alla root poi le categorie sotto i reparti*/
            long rootCateg = listCategories.Where(a => a.is_root_category == 1).First().id.Value;

            using (UnitOfWork uof = new UnitOfWork())
            {
                foreach (var reparto in uof.CategorieRepository.Find(a => a.Codice > 0).Select(a => a.Reparto)
                    .Distinct().OrderBy(a => a).ToList())
                {
                    /*verifico che il reparto sia presente sotto la root*/
                    var repartoWeb = listCategories.Where(a => a.name.First().Value == reparto).DefaultIfEmpty(null).FirstOrDefault();

                    if (repartoWeb == null)
                    {
                        retVal = true;
                        //se non si trova va aggiunto
                        AggiungiRepartoWeb(rootCateg, reparto);
                    }
                    else
                    {
                        //aggiorno o salvo l'info a db
                        var repartoDbWeb = uof.RepartoWebRepository.Find(a => a.Reparto == reparto).FirstOrDefault();
                        if (repartoDbWeb == null)
                        {
                            repartoDbWeb = new Library.Entity.Ecomm.RepartoWeb() { Reparto = reparto, CodiceWeb = repartoWeb.id.Value };
                            uof.RepartoWebRepository.Add(repartoDbWeb);
                            uof.Commit();
                        }
                        else if (repartoDbWeb.CodiceWeb != repartoWeb.id.Value)
                        {
                            repartoDbWeb.CodiceWeb = repartoWeb.id.Value;
                            uof.RepartoWebRepository.Update(repartoDbWeb);
                            uof.Commit();
                        }
                        else
                        {
                            ManagerLog.AddLogMessage("Reparto presente e corretto : " + reparto);
                        }
                    }
                };
            }
            if (retVal)
            {
                listCategories = LeggiCategorieDalWeb();

            }
            return listCategories;
        }

        private List<category> LeggiCategorieDalWeb()
        {
            Core.Manager.ManagerLog.AddLogMessage("Avvio lettura categorie dal web");
            var listCategories = new List<category>();


            foreach (var item in _categoriesFact.GetAll())
            {
                listCategories.Add((item));
                //Core.Manager.ManagerLog.AddLogObject(item, "categoria:");

            }
            Core.Manager.ManagerLog.AddLogObject(listCategories.Select(a =>
            new { a.id, a.id_parent, a.is_root_category, Nome = a.name.First().Value }).ToList(), "categorie:");
            Core.Manager.ManagerLog.AddLogMessage("Termine lettura categorie dal web");

            return listCategories;
        }

        private void AggiungiRepartoWeb(long rootCateg, string reparto)
        {
            category newCateg = AggiungiCategoriaWeb(rootCateg, reparto);

            using (var uof = new UnitOfWork())
            {
                uof.RepartoWebRepository.Add(new Library.Entity.Ecomm.RepartoWeb() { Reparto = reparto, CodiceWeb = newCateg.id.Value });
                uof.Commit();
            }
        }

        private category AggiungiCategoriaWeb(long rootCateg, string nomeCateg)
        {
            category newCateg = (new category() { active = 1, id_parent = rootCateg });
            newCateg.AddName(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, nomeCateg));
            newCateg.AddLinkRewrite(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, nomeCateg));

            _categoriesFact.Add(newCateg);

            Dictionary<string, string> dtn = new Dictionary<string, string>();
            dtn.Add("name", nomeCateg);
            var listFind = _categoriesFact.GetByFilter(dtn, null, null);
            listFind = listFind.Where(a => a.id_parent == rootCateg).ToList();
            if (listFind.Count() == 1)
            {
                newCateg = listFind.First();
            }
            else
            {
                ManagerLog.AddLogMessage("C'è un problema con la categoria'" + nomeCateg + "' nel web");
            }

            return newCateg;
        }

        private void CaricaArticolo(Articolo artDb)
        {
            using (var uof = new UnitOfWork())
            {
                product artWeb = new product();
                artWeb.price = artDb.Prezzo;
                ManagerLog.AddLogMessage("Caricamento in corso dell'articolo '" + artDb.Titolo + "' ID=" + artDb.ID + "  nel web");

                artWeb.AddName(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.Titolo));
                artWeb.AddLinkRewrite(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.Titolo));
                artWeb.active = 1;
                artWeb.condition = "new";
                artWeb.show_condition = 1;
                //*iva al 22%*//
                artWeb.id_tax_rules_group= 1;
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
                artWeb.price = artDb.Prezzo;
                artWeb.show_price = 1;

                ///*categoria non specificato*/
                //artWeb.id_category_default= 239;
                //if (categDb != null)
                //    articolo.CategoriaID = categDb.ID;
                artWeb.id_category_default = uof.CategorieWebRepository.Find(a => a.CategoriaID == artDb.CategoriaID).FirstOrDefault().CodiceWeb;

                var categDb = uof.CategorieRepository.Find(a => a.ID == artDb.CategoriaID).First();

                var listaCateg = uof.CategorieWebRepository.Find(a => a.Categoria.Nome == categDb.Nome).Select(a => a.CodiceWeb).ToList();

                foreach (var item in listaCateg)
                {
                    artWeb.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(item));

                }

                CaricaImmagini(artDb, artWeb);

                if (string.IsNullOrEmpty(artDb.ArticoloWeb.DescrizioneHtml))
                    artDb.ArticoloWeb.DescrizioneHtml = "";
                artWeb.description.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1,
                artDb.ArticoloWeb.DescrizioneHtml));

                if (string.IsNullOrEmpty(artDb.ArticoloWeb.DescrizioneBreveHtml))
                    artDb.ArticoloWeb.DescrizioneBreveHtml = "";
                artWeb.description_short.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1,
                    artDb.ArticoloWeb.DescrizioneBreveHtml));
                artWeb.available_for_order = 1;
                artWeb.New = 1;
                artWeb.state = 1;
                artWeb.visibility = "both";
                artWeb.minimal_quantity = 1;
                artWeb.reference = artDb.ID.ToString();
                try
                {
                    var newArtWeb = _productFactory.Add(artWeb);
                    var stock = _StockAvailableFactory.Get(newArtWeb.associations.stock_availables.First().id);
                    stock.quantity = 100;
                    _StockAvailableFactory.Update(stock);
                }
                catch (Exception ex)
                {
                    throw ex;
                }





            }
        }

        private void CaricaImmagini(Articolo artDb, product artWeb)
        {

        }

        /*leggo i prodotti che sono da aggiornare */
        public void ReadAllFromSite()
        {

            var productFactory = new ProductFactory(_url, _autKey, "");

            var listCategories = LeggiCategorieDalWeb();

            //Dictionary<string, string> filter = new Dictionary<string, string>();
            //filter.Add("Name", "EKO RANGER");
            var listProd = new List<product>();

            foreach (var item in productFactory.GetIds().OrderBy(a => a).Where(a => a < 20).ToList())
            {
                var prod = productFactory.Get(item);
                listProd.Add(prod);
                Console.WriteLine("Prodotto :" + prod.name.First().Value);
            }

            var titoli = listProd.Select(a => new { Titolo = a.name.First().Value, id = a.id })
                .OrderBy(a => a.Titolo).ToList();


            using (var uof = new UnitOfWork())
            {
                foreach (var item in listProd)
                {
                    var codice = item.id.Value.ToString();
                    var name = item.name.First().Value;
                    var articolo = uof.ArticoliRepository.Find(a => a.ArticoloWeb.CodiceArticoloEcommerce == codice).FirstOrDefault();
                    if (articolo == null)
                    {

                        //articolo = uof.ArticoliRepository.Find(a => a.Titolo == name).FirstOrDefault();
                        if (articolo == null)
                        {
                            try
                            {
                                SalvaArticolo(uof, item, name, listCategories);
                                Console.WriteLine("Salvato nuovo articolo dal web:" + name);

                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine("Errore:" + ex.Message);

                                Console.WriteLine("Vuoi continuare ? Y/N");
                                if (Console.ReadLine().ToUpperInvariant() != "Y")
                                {
                                    return;
                                }
                            }
                            //                            articolo.Categoria=item.cate
                        }
                        else
                        {
                            Console.WriteLine("Articolo già presente per nome:" + name);
                        }
                    }
                    else
                    {

                        Console.WriteLine("Articolo già presente per codice:" + name);
                    }
                    //item.condition
                }
            }
        }

        private void SalvaArticolo(UnitOfWork uof, product item, string name, List<category> listCategories)
        {
            Articolo articolo = new Articolo
            {
                Prezzo = item.price,
                Titolo = name,
                Condizione = enCondizioneArticolo.Nuovo,
                Testo = name

            };
            articolo.TagImport = "SitoWeb";
            if (item.condition.Contains("new"))
            {
                articolo.Condizione = enCondizioneArticolo.Nuovo;
            }
            else if (item.condition.Contains("use"))
            {
                articolo.Condizione = enCondizioneArticolo.UsatoGarantito;
            }
            else
            {

            }
            var categEcommerce = listCategories.Where(a => item.id_category_default.Value == a.id.Value).FirstOrDefault();
            var categName = categEcommerce.name.First().Value;
            var categDb = uof.CategorieRepository.Find(a => a.Nome
             == categName).FirstOrDefault();

            /*categoria non specificato*/
            articolo.CategoriaID = 239;
            if (categDb != null)
                articolo.CategoriaID = categDb.ID;

            var immagini = LeggiImmagini(item);

            articolo.ArticoloWeb.DescrizioneHtml = item.description.FirstOrDefault().Value;
            articolo.ArticoloWeb.DescrizioneBreveHtml = item.description_short.FirstOrDefault().Value;
            articolo.ArticoloWeb.CodiceArticoloEcommerce = item.id.Value.ToString();

            uof.ArticoliRepository.Add(articolo);
            uof.Commit();
            if (immagini.Count() > 0 && !ImageArticoloSave.AddImageFiles(
                new ImageArticoloAddFiles(articolo, immagini, new ControllerFake())))
                throw new MessageException("Non si sono salvati correttamente le immagini degli articoli");

        }
        private class ControllerFake : IKeyController
        {
            public Guid INSTANCE_KEY => Guid.NewGuid();
        }
        private List<string> LeggiImmagini(product item)
        {
            List<string> immagini = new List<string>();
            long idDefaultImage = -1;
            if (item.id_default_image.HasValue)
            {
                idDefaultImage = item.id_default_image.Value;
                var image = byteArrayToImage(_imageFactory.GetProductImage(item.id.Value, idDefaultImage)
                    );
                immagini.Add(image);
                //fotoArticolo.UrlFoto

                //image.Save()
            }
            foreach (var itemImage in item.associations.images.Where(a => a.id != idDefaultImage))
            {
                var image = byteArrayToImage(_imageFactory.GetProductImage(item.id.Value, itemImage.id)
                    );
                immagini.Add(image);
            }
            return immagini;
        }

        public string byteArrayToImage(byte[] bytesArr)
        {

            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                var img = new ImageMagick.MagickImage(bytesArr);
                var atempFile = System.IO.Path.GetTempFileName() + ".jpg";
                img.Write(atempFile);
                return atempFile;
            }
        }
        private PrestaShopSetting LoginData()
        {
            using (var uof = new UnitOfWork())
            {
                var setting = uof.SettingSitoRepository.Find(a => 1 == 1).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingSito();
                    uof.SettingSitoRepository.Add(setting);
                    uof.Commit();
                }
                return setting.prestaShopSetting;
            }
        }
    }
}
