using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.Library.Entity.Ecomm;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.PrestaShopSyncro.Products
{
    public class BaseProduct : BaseClass.SyncroBase
    {
        internal product GetProdWebFromCodartEcommerce(ArticoloBase artDb)
        {
            return _productFactory.Get(long.Parse(artDb.CodiceArticoloEcommerce));
        }
        internal void SalvaAggiornamento(UnitOfWork uof, AggiornamentoWebArticolo aggiornamento)
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
    }
}
