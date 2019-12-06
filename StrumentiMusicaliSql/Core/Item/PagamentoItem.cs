using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Item
{
    public class PagamentoItem : BaseItem<Pagamento>
    {
        public PagamentoItem()
        {

        }
        public PagamentoItem(Pagamento pagamento)
        {
            Nome = pagamento.Nome;
            Cognome = pagamento.Cognome;
            DataInizio = pagamento.DataInizio.Date;
            Articolo = pagamento.ArticoloAcq;
            DataRata = pagamento.DataRata.Date;
            ImportoRata = pagamento.ImportoRata;
            ImportoResiduo = pagamento.ImportoResiduo;
            ImportoTotale = pagamento.ImportoTotale;


            Entity = pagamento;
            ID = pagamento.ID;
        }
        public string Nome { get; set; }
        public string Cognome { get; }
        public DateTime DataInizio { get; }
        public string Articolo { get; }
        public decimal ImportoRata { get; }
        public DateTime DataRata { get; }
        public decimal ImportoResiduo { get; }
        public decimal ImportoTotale { get; }
    }
}
