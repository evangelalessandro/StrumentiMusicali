using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Item
{
    public class ClientiItem : BaseItem<Cliente>
    {
        public ClientiItem()
            : base()
        {
        }
        public ClientiItem(Cliente item)
            : base()
        {
            ID = item.ID;


            PIVA = item.PIVA;
            if (string.IsNullOrEmpty( PIVA))
            {
                PIVA= item.CodiceFiscale;
            }
            RagioneSociale = item.RagioneSociale;
            if (string.IsNullOrEmpty( RagioneSociale))
            {
                RagioneSociale = item.Cognome + " " + item.Nome;
            }
            Via = item.Indirizzo.IndirizzoConCivico;
            DataCreazione = item.DataCreazione;
            Citta = item.Indirizzo.Citta;
            Telefono = item.Telefono;
            Fax = item.Fax;
            Cellulare = item.Cellulare;

        }

        public string PIVA { get; set; }
        public string RagioneSociale { get; set; }

        public string Via { get; set; }
        public string Citta { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Cellulare { get; set; }

        public DateTime DataCreazione { get; set; }
    }
}
