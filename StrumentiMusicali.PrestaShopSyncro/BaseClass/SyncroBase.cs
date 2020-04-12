using Bukimedia.PrestaSharp.Factories;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro.BaseClass
{
    public abstract class SyncroBase : IDisposable
    {
        private string _url = "";
        private string _autKey = "";

        public SyncroBase()
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
            _taxRuleGroupFact=new TaxRuleGroupFactory(_url, _autKey, "");
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

        protected TaxRuleGroupFactory _taxRuleGroupFact = null;

        protected CategoryFactory _categoriesFact = null;
        protected StockAvailableFactory _StockAvailableFactory = null;
        protected ImageFactory _imageFactory = null;
        protected ProductFactory _productFactory = null;

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