
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.EcommerceBaseSyncro.Base;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.EcommerceBaseSyncro
{
    public abstract class StockProductsBase : SyncroBase
    {
        /// <summary>
        /// Aggiorna il magazzino del articolo nel web
        /// </summary>
        /// <param name="newArtWeb"></param>
        /// <param name="artDb"></param>
        public static bool UpdateStockArt(ref int qta , ArticoloBase artDb, UnitOfWork uof, bool forzaUpdate=false)
        {
            
            if (!forzaUpdate && artDb.Aggiornamento.GiacenzaMagazzinoWebInDataAggWeb != qta)
            {
                ManagerLog.Logger.Info("Il prodotto '" + artDb.ArticoloDb.Titolo +
                    "' è stato modificato con gli ordini nel web, occorre prima aspettare che venga " +
                    "scaricata la nuova qta dal job delle giacenze.");
                return false;
            }
            DateTime date = DateTime.Now;
            qta = CalcolaStock(artDb);
            
            artDb.Aggiornamento.DataUltimoAggMagazzino = date;
            artDb.Aggiornamento.DataUltimoAggMagazzinoWeb = date;
            artDb.Aggiornamento.GiacenzaMagazzinoWebInDataAggWeb = qta;
            SalvaAggiornamento(uof, artDb.Aggiornamento);
            return true;
        }

        public List<ArticoloBase> UpdateStock(UnitOfWork uof)
        {
            var list = base.ListArt(uof,true);
            List<ArticoloBase> listUpdated = new List<ArticoloBase>();
            foreach (var item in list)
            {
                if (UpdateStockArt(item, uof))
                    listUpdated.Add(item);
            }
            return listUpdated;
        }

        public static int CalcolaStock(ArticoloBase artDb)
        {
            using (var uof = new UnitOfWork())
            {
                var giacenza = uof.MagazzinoRepository.Find(a => artDb.ArticoloID.Equals(a.ArticoloID))
                               .Select(a => new { a.ArticoloID, a.Qta, a.Deposito }).GroupBy(a => new { a.ArticoloID, a.Deposito })
                               .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key }).ToList();

                 

                return giacenza.Where(a => a.Articolo.Deposito.Principale).Select(a => a.Sum)
                        .DefaultIfEmpty(0).FirstOrDefault();
                 
            }
        }

        public abstract bool UpdateStockArt(ArticoloBase artDb, UnitOfWork uof);
         
    }
}