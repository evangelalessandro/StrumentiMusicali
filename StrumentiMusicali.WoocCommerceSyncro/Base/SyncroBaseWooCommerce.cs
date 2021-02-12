
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace StrumentiMusicali.WooCommerceSyncro.BaseClass
{
    public abstract class SyncroBaseWooCommerce :   EcommerceBaseSyncro.Base.SyncroBase ,IDisposable
    {
        //protected string _url = "";
        //protected string _autKey = "";
        internal WCObject _wc;
        public SyncroBaseWooCommerce()
        {
            var dati=LoginData();
            
            RestAPI rest = new RestAPI(dati.WebServiceUrl,
                dati.keyPublic, dati.keyPrivate//"cs_691cad866d216a0c85f3d56aacc0a55c92165a21"
                );
            _wc = new WCObject(rest);

             

     
        }
        public Product UpdateProdWeb(Product artWeb)
        {
            if (!artWeb.id.HasValue)
            {
                var tsk = _wc.Product.Add(artWeb);
                tsk.Wait();
                artWeb = tsk.Result;
            }
            else
            {
                var tsk = _wc.Product.Update(artWeb.id.Value, artWeb, null);
                tsk.Wait();
                artWeb = tsk.Result;
            }

            return artWeb;
        }
        public WooCommerceNET.WooCommerce.v3.Product GetProdWeb(ArticoloBase artDb, out bool newProd)
        {
            var artWeb = new WooCommerceNET.WooCommerce.v3.Product();
            newProd = false;
            if (!string.IsNullOrEmpty(artDb.Aggiornamento.CodiceArticoloEcommerce))
            {
                var tsk = _wc.Product.Get(int.Parse(artDb.Aggiornamento.CodiceArticoloEcommerce), null);
                tsk.Wait();
                artWeb = tsk.Result;
            }
            else
            {
                var parms = new Dictionary<string, string>();
                parms.Add("sku", artDb.ArticoloID.ToString());

                var tsk = _wc.Product.GetAll(parms);
                tsk.Wait();
                artWeb = tsk.Result.FirstOrDefault();
                if (artWeb == null)
                {
                    artWeb = new WooCommerceNET.WooCommerce.v3.Product();
                    newProd = true;
                }
            }
            return artWeb;
        }
        private WooCommerceSetting LoginData()
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
                return setting.WooCommerceSetting;
            }
        }

        //private static bool _checkedDebugMode = false;
        //private static void CheckModeDebug(Configuration config)
        //{
        //    if (_checkedDebugMode)
        //        return;
        //    _checkedDebugMode = true;
        //    var setting = config.AppSettings.Settings.AllKeys.Where(a => a == "Test").FirstOrDefault();

        //    if (setting == null)
        //    {
        //        config.AppSettings.Settings.Add("Test", StrumentiMusicali.PrestaShopSyncro.Properties.Resources.Test);
        //        config.Save(ConfigurationSaveMode.Modified);
        //        ConfigurationManager.RefreshSection("appSettings");
        //    }
        //    setting = config.AppSettings.Settings.AllKeys.Where(a => a == "Test").FirstOrDefault();


        //    setting = config.AppSettings.Settings.AllKeys.Where(a => a == "AutKey").FirstOrDefault();
        //    if (setting == null)
        //    {
        //        config.AppSettings.Settings.Add("AutKey", StrumentiMusicali.PrestaShopSyncro.Properties.Resources.AutKey);
        //        config.Save(ConfigurationSaveMode.Modified);
        //        ConfigurationManager.RefreshSection("appSettings");
        //    }
        //    setting = config.AppSettings.Settings.AllKeys.Where(a => a == "UrlPrestaShop").FirstOrDefault();
        //    if (setting == null)
        //    {
        //        config.AppSettings.Settings.Add("UrlPrestaShop", StrumentiMusicali.PrestaShopSyncro.Properties.Resources.UrlPrestaShop);
        //        config.Save(ConfigurationSaveMode.Modified);
        //        ConfigurationManager.RefreshSection("appSettings");
        //    }


        //}


        #region IDisposable Support
        private bool disposedValue = false; // Per rilevare chiamate ridondanti

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminare lo stato gestito (oggetti gestiti).
                    _wc = null;
                }
               

                // TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
                // TODO: impostare campi di grandi dimensioni su Null.

                disposedValue = true;
            }
        }

        // TODO: eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
        // ~SyncroBase()
        // {
        //   // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
        //   Dispose(false);
        // }

        // Questo codice viene aggiunto per implementare in modo corretto il criterio Disposable.
        public void Dispose()
        {
            // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
            Dispose(true);
            // TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override del finalizzatore.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}