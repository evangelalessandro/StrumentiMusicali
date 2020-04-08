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
            _url = "http://localhost:10080/prestashop/api/";
            _imageFactory = new ImageFactory(_url, _autKey, "");
            _StockAvailableFactory = new StockAvailableFactory(_url, _autKey, "");
            _productFactory = new ProductFactory(_url, _autKey, "");
        }

        StockAvailableFactory _StockAvailableFactory = null;
        ImageFactory _imageFactory = null;
        List<category> _listCategories = new List<category>();
        ProductFactory _productFactory = null;

        /// <summary>
        /// Tutti gli articoli che sono con il flag CaricainECommerce e 
        /// </summary>
        public void AggiornaWeb()
        {
            using (var uof = new UnitOfWork())
            {
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

        private void CaricaArticolo(Articolo artDb)
        {
            using (var uof = new UnitOfWork())
            {
                product artWeb = new product();
                artWeb.price = artDb.Prezzo;
                artWeb.AddName(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.Titolo));
                artWeb.AddLinkRewrite(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, artDb.Titolo));

                artWeb.condition = "new";
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

                ///*categoria non specificato*/
                //artWeb.id_category_default= 239;
                //if (categDb != null)
                //    articolo.CategoriaID = categDb.ID;
                artWeb.id_category_default = 8;

                artWeb.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(8));
                artWeb.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(3));

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
                //artWeb.id_default_image.images.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.image())
                //artWeb.New = 200;
                //artWeb.New= artDb.ArticoloWeb.CodiceArticoloEcommerce;
                try
                {
                    _productFactory.Add(artWeb);
                }
                catch (Exception ex)
                {
                    throw ex;
                }




                _StockAvailableFactory.Add(new stock_available() { id_product = artWeb.id, id_shop = 1, quantity = 100 });

            }
        }

        private void CaricaImmagini(Articolo artDb, product artWeb)
        {

        }

        /*leggo i prodotti che sono da aggiornare */
        public void ReadAllFromSite()
        {

            var productFactory = new ProductFactory(_url, _autKey, "");

            var categoriesFact = new CategoryFactory(_url, _autKey, "");
            var categoriesIds = categoriesFact.GetIds();

            foreach (var item in categoriesIds)
            {
                _listCategories.Add(categoriesFact.Get(item));
                System.Console.WriteLine(_listCategories.Last().name.FirstOrDefault().Value);
            }
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
                                SalvaArticolo(uof, item, name);
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

        private void SalvaArticolo(UnitOfWork uof, product item, string name)
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
            var categEcommerce = _listCategories.Where(a => item.id_category_default.Value == a.id.Value).FirstOrDefault();
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
