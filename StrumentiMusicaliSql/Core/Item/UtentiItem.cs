using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Item
{
    public class UtentiItem : BaseItem<Utente>
    {
        public UtentiItem()
            : base()
        {
        }
        public UtentiItem(Utente item)
            : base()
        {
            ID = item.ID;


            NomeUtente = item.NomeUtente;
            AdminUtenti = item.AdminUtenti;
            Fatturazione = item.Fatturazione;
            Magazzino = item.Magazzino;
            //ScontaArticoli = item.ScontaArticoli;
            DataCreazione = item.DataCreazione;

        }

        public string NomeUtente { get; set; }
        public bool AdminUtenti { get; set; }

        public bool Fatturazione { get; set; }

        //public bool ScontaArticoli { get; set; }

        public bool Magazzino { get; set; }

        public DateTime DataCreazione { get; set; }
    }
}
