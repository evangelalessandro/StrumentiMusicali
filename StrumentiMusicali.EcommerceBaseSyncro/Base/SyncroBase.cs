using StrumentiMusicali.Library.Entity.Ecomm;
using StrumentiMusicali.Library.Entity.Enums;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.EcommerceBaseSyncro.Base
{
    public abstract class SyncroBase
    {

        public static void SalvaAggiornamento(UnitOfWork uof, AggiornamentoWebArticolo aggiornamento)
        {
            if (aggiornamento.ID == 0)
            {
                uof.AggiornamentoWebArticoloRepository.Add(aggiornamento);
            }
            else
            {
                uof.AggiornamentoWebArticoloRepository.Update(aggiornamento);
            }
            uof.Commit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uof"></param>
        /// <param name="stock">Filtro per data modifica stock non allineata</param>
        /// <returns></returns>
        public List<ArticoloBase> ListArt(UnitOfWork uof, bool stock, bool foto=false)
        {
            var dati = uof.AggiornamentoWebArticoloRepository.Find(a => (a.Articolo.CaricainECommerce

                                     && (a.Articolo.Categoria.Codice >= 0
                        || a.Articolo.Condizione == enCondizioneArticolo.ExDemo
                        || a.Articolo.Condizione == enCondizioneArticolo.UsatoGarantito)
                               && a.Articolo.ArticoloWeb.PrezzoWeb > 0
                               )
                               || a.ForzaAggiornamento == true).
                              Select(a => new ArticoloBase
                              {
                                  CodiceArticoloEcommerce =
                                    a.CodiceArticoloEcommerce,
                                  ArticoloID = a.Articolo.ID,
                                  Aggiornamento = a,
                                  ArticoloDb = a.Articolo
                              }).ToList();
            if (stock)
            {
                dati = dati.Where(a => Math.Abs((a.Aggiornamento.DataUltimoAggMagazzino - a.Aggiornamento.DataUltimoAggMagazzinoWeb)
                                 .TotalSeconds) > 10
                                 || a.Aggiornamento.ForzaAggiornamento == true)
                                .ToList();
            }
            else
            {

                dati = dati.Where(a => Math.Abs((a.Aggiornamento.DataUltimoAggiornamentoWeb - a.ArticoloDb.DataUltimaModifica).TotalSeconds) > 10
                        || (foto && Math.Abs((a.Aggiornamento.DataUltimoAggFotoWeb - a.Aggiornamento.DataUltimoAggFoto).TotalSeconds) > 10)

                    || a.Aggiornamento.ForzaAggiornamento == true).ToList();

            }

            return dati;
        }
    }
}

