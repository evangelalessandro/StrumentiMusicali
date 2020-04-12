using Bukimedia.PrestaSharp.Entities;
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

namespace StrumentiMusicali.PrestaShopSyncro.Products
{
    internal class ReadProductsFormSite : BaseClass.SyncroBase
    {
         /// <summary>
         /// Salva quelli che non sono presenti in locale
         /// </summary>
         /// <returns>ritorna quanti sono stati inseriti</returns>
        public int SaveLocalFromSite()
        {

            int count = 0;
            using (var uof = new UnitOfWork())
            {
                using (var categ = new CategorySync())
                {
                    var listCategories = categ.LeggiCategorieDalWeb();

                    //Dictionary<string, string> filter = new Dictionary<string, string>();
                    //filter.Add("Name", "EKO RANGER");
                    var listProd = new List<product>();

                    var codiciGiaPresenti = uof.ArticoliRepository.Find(a => 1 == 1).Select(a => a.ArticoloWeb.CodiceArticoloEcommerce).ToList();
                    var listIdProdWeb = _productFactory.GetIds().OrderBy(a => a).Select(a => a.ToString()).ToList();
                    //prendo quelli che non sono già salvati
                    foreach (var item in codiciGiaPresenti.Where(a => !listIdProdWeb.Contains(a)))
                    {
                        var prod = _productFactory.Get(long.Parse(item));
                        listProd.Add(prod);
                        Console.WriteLine("Prodotto :" + prod.name.First().Value);
                    }
                    foreach (var item in listProd)
                    {
                        var codice = item.id.Value.ToString();

                        var name = item.name.First().Value;
                        var articolo = uof.ArticoliRepository.Find(a => a.ArticoloWeb.CodiceArticoloEcommerce == codice).FirstOrDefault();
                        if (articolo == null)
                        {
                            try
                            {
                                SaveProduct(uof, item, name, listCategories);
                                Console.WriteLine("Salvato nuovo articolo dal web:" + name);
                                count++;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Errore:" + ex.Message);

                                Console.WriteLine("Vuoi continuare ? Y/N");
                                if (Console.ReadLine().ToUpperInvariant() != "Y")
                                {
                                    return count;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Articolo già presente per codice:" + name);
                        }
                    }
                }
            }
            return count;
        }

        #region Immagini

        private string byteArrayToImage(byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                var img = new ImageMagick.MagickImage(bytesArr);
                var atempFile = System.IO.Path.GetTempFileName() + ".jpg";
                img.Write(atempFile);
                return atempFile;
            }
        }

        private List<string> ReadImmage(product item)
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

        #endregion Immagini

        private void SaveProduct(UnitOfWork uof, product item, string name, List<category> listCategories)
        {
            Articolo articolo = new Articolo
            {
                Titolo = name,
                Condizione = enCondizioneArticolo.Nuovo,
                Testo = name
            };
            articolo.ArticoloWeb.Iva = 22;

            articolo.Prezzo = item.wholesale_price;
            articolo.ArticoloWeb.PrezzoWeb = item.wholesale_price;

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

            var immagini = ReadImmage(item);

            articolo.ArticoloWeb.DescrizioneHtml = item.description.FirstOrDefault().Value;
            articolo.ArticoloWeb.DescrizioneBreveHtml = item.description_short.FirstOrDefault().Value;
            articolo.ArticoloWeb.CodiceArticoloEcommerce = item.id.Value.ToString();

            articolo.CaricaInMercatino = false;
            articolo.CaricainECommerce = false;

            uof.ArticoliRepository.Add(articolo);

            uof.Commit();

            item.reference = articolo.ID.ToString();
            _productFactory.Update(item);

            var depoPrinc = uof.DepositoRepository.Find(a => a.Principale == true).First();

            uof.MagazzinoRepository.Add(new Library.Entity.Magazzino()
            { ArticoloID = articolo.ID, DepositoID = depoPrinc.ID, Qta = 1, PrezzoAcquisto = 0 });

            uof.Commit();

            if (immagini.Count() > 0 && !ImageArticoloSave.AddImageFiles(
                new ImageArticoloAddFiles(articolo, immagini, new ControllerFake())))
                throw new MessageException("Non si sono salvati correttamente le immagini degli articoli");
        }

        private class ControllerFake : IKeyController
        {
            public Guid INSTANCE_KEY => Guid.NewGuid();
        }
    }
}