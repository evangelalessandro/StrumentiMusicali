using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace StrumentiMusicali.PrestaShopSyncro.BaseClass
{
    public abstract class SyncroBasePresta : EcommerceBaseSyncro.Base.SyncroBase, IDisposable
    {
        protected string _url = "";
        protected string _autKey = "";
        internal product GetProdWebFromCodartEcommerce(ArticoloBase artDb)
        {
            return _productFactory.Get((artDb.CodiceArticoloEcommerce));
        }
        public SyncroBasePresta()
            :base()
        {
            var logondata = LoginData();
            _url = logondata.WebServiceUrl;
            _url += "api/";
            _autKey = logondata.AuthKey;
            var config = ConfigurationManager.OpenExeConfiguration(
                Assembly.GetExecutingAssembly().Location);
            CheckModeDebug(config);
            //if (config.AppSettings.Settings["Test"].Value == "1")
            //{
            //    _url = config.AppSettings.Settings["UrlPrestaShop"].Value;
            //    _autKey = config.AppSettings.Settings["AutKey"].Value;

            //    //StrumentiMusicali.Core.Manager.ManagerLog.Logger.Info("TEST _url ");
            //    //StrumentiMusicali.Core.Manager.ManagerLog.Logger.Info(_url);
            
            //}

            _imageFactory = new ImageFactory(_url, _autKey, "");
            _StockAvailableFactory = new StockAvailableFactory(_url, _autKey, "");
            _productFactory = new ProductFactory(_url, _autKey, "");
            _categoriesFact = new CategoryFactory(_url, _autKey, "");
            _taxRuleGroupFact = new TaxRuleGroupFactory(_url, _autKey, "");
			_taxFact = new TaxFactory(_url, _autKey, "");

		}
        
        private static bool _checkedDebugMode = false;
        private static void CheckModeDebug(Configuration config)
        {
			return;
            if (_checkedDebugMode)
                return;
            _checkedDebugMode = true;
            var setting = config.AppSettings.Settings.AllKeys.Where(a => a == "Test").FirstOrDefault();

            if (setting == null)
            {
                config.AppSettings.Settings.Add("Test", StrumentiMusicali.PrestaShopSyncro.Properties.Resources.Test);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            setting = config.AppSettings.Settings.AllKeys.Where(a => a == "Test").FirstOrDefault();


            setting = config.AppSettings.Settings.AllKeys.Where(a => a == "AutKey").FirstOrDefault();
            if (setting == null)
            {
                config.AppSettings.Settings.Add("AutKey", StrumentiMusicali.PrestaShopSyncro.Properties.Resources.AutKey);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            setting = config.AppSettings.Settings.AllKeys.Where(a => a == "UrlPrestaShop").FirstOrDefault();
            if (setting == null)
            {
                config.AppSettings.Settings.Add("UrlPrestaShop", StrumentiMusicali.PrestaShopSyncro.Properties.Resources.UrlPrestaShop);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
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
                return setting.PrestaShopSetting;
            }
        }

        internal TaxRuleGroupFactory _taxRuleGroupFact = null;
		internal TaxFactory _taxFact = null;

		internal CategoryFactory _categoriesFact = null;
        internal StockAvailableFactory _StockAvailableFactory = null;
        internal ImageFactory _imageFactory = null;
        internal ProductFactory _productFactory = null;

        #region IDisposable Support
        private bool disposedValue = false; // Per rilevare chiamate ridondanti

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminare lo stato gestito (oggetti gestiti).
                }
                _categoriesFact = null;
                _StockAvailableFactory = null;
                _imageFactory = null;
                _productFactory = null;
				_taxFact = null;
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