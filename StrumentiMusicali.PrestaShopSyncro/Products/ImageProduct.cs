using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.PrestaShopSyncro.Products
{
    internal class ImageProduct : BaseProduct
    {

        private void UpdateImage(ArticoloBase artDb, product artWeb, UnitOfWork uof)
        {
            DateTime date = DateTime.Now;
            /*cancello le immagini e le sovrascrivo*/
            foreach (var item in artWeb.associations.images)
            {
                _imageFactory.DeleteProductImage(artWeb.id.Value, item.id);
            }
            var settingSito = SettingSitoValidator.ReadSetting();

            var imageList = uof.FotoArticoloRepository.Find(a => a.ArticoloID == artDb.ArticoloID).OrderBy(a => a.Ordine).ToList();

            var listFotoArticolo = imageList.Select(a => new ImmaginiFile<FotoArticolo>(Path.Combine(
                settingSito.CartellaLocaleImmagini, a.UrlFoto)
                             , a.UrlFoto, a)).ToList();

            foreach (var item in listFotoArticolo)
            {
                _imageFactory.AddProductImage(artWeb.id.Value, item.File);
            }
            var aggiornamento = artDb.Aggiornamento;

            aggiornamento.DataUltimoAggFoto = date;
            aggiornamento.DataUltimoAggFotoWeb = date;
            SalvaAggiornamento(uof, aggiornamento);
        }

        public void UpdateImages(UnitOfWork uof)
        {
            var aggiornamentoWebs = uof.AggiornamentoWebArticoloRepository.Find(a => a.Articolo.CaricainECommerce

                               && a.Articolo.Categoria.Codice >= 0
                              && a.Articolo.ArticoloWeb.PrezzoWeb > 0).
                              Select(a => new ArticoloBase
                              {
                                  CodiceArticoloEcommerce =
                              a.CodiceArticoloEcommerce,
                                  ArticoloID = a.Articolo.ID,
                                  Aggiornamento = a,
                                  ArticoloDb = a.Articolo
                              }).ToList()
                                .Where(a => Math.Abs((a.Aggiornamento.DataUltimoAggFoto - a.Aggiornamento.DataUltimoAggFotoWeb)
                                .TotalSeconds) > 10)
                                .ToList();
            foreach (var artDb in aggiornamentoWebs)
            {
                if (!string.IsNullOrEmpty(artDb.CodiceArticoloEcommerce))
                {
                    product artWeb = GetProdWebFromCodartEcommerce(artDb);

                    UpdateImage(artDb, artWeb, uof);

                }
            }
        }
    }
}
