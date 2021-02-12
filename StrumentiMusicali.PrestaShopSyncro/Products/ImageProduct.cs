using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.EcommerceBaseSyncro.Base;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.PrestaShopSyncro.BaseClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.PrestaShopSyncro.Products
{
    internal class ImageProduct : SyncroBase, IDisposable
    {
        SyncroBasePresta _syncroBasePresta;
        public ImageProduct(SyncroBasePresta syncroBasePresta)
        {
            _syncroBasePresta = syncroBasePresta;
        }
        public void Dispose()
        {
            if (_syncroBasePresta != null)
                _syncroBasePresta.Dispose();

        }
        private void UpdateImage(ArticoloBase artDb, product artWeb, UnitOfWork uof)
        {
            DateTime date = DateTime.Now;
            /*cancello le immagini e le sovrascrivo*/
            foreach (var item in artWeb.associations.images)
            {
                _syncroBasePresta._imageFactory.DeleteProductImage(artWeb.id.Value, item.id);
            }
            var settingSito = SettingSitoValidator.ReadSetting();

            var imageList = uof.FotoArticoloRepository.Find(a => a.ArticoloID == artDb.ArticoloID).OrderBy(a => a.Ordine).ToList();

            var listFotoArticolo = imageList.Select(a => new ImmaginiFile<FotoArticolo>(Path.Combine(
                settingSito.CartellaLocaleImmagini, a.UrlFoto)
                             , a.UrlFoto, a)).ToList();

            foreach (var item in listFotoArticolo)
            {
                _syncroBasePresta._imageFactory.AddProductImage(artWeb.id.Value, item.File);
            }
            var aggiornamento = artDb.Aggiornamento;

            aggiornamento.DataUltimoAggFoto = date;
            aggiornamento.DataUltimoAggFotoWeb = date;
            SalvaAggiornamento(uof, aggiornamento);
        }

        public List<ArticoloBase> UpdateImages(UnitOfWork uof)
        {
            var aggiornamentoWebs = base.ListArt(uof,false);
            foreach (var artDb in aggiornamentoWebs)
            {
                if (!string.IsNullOrEmpty(artDb.CodiceArticoloEcommerce))
                {
                    product artWeb = _syncroBasePresta.GetProdWebFromCodartEcommerce(artDb);

                    UpdateImage(artDb, artWeb, uof);

                }
            }
            return aggiornamentoWebs;
        }
    }
}
