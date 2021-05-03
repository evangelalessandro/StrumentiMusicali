using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Core.Item
{
    public class ListinoPrezziVenditaNomeItem : BaseItem<ListinoPrezziVenditaNome>
    {

        public ListinoPrezziVenditaNomeItem() { }
        public ListinoPrezziVenditaNomeItem(ListinoPrezziVenditaNome item)
        {
            Nome = item.Nome;
            DataInizioValidita = item.DataInizioValidita;
            DataFineValidita = item.DataFineValidita;
            PercentualeVariazione = item.PercentualeVariazione;

        }
        public string Nome { get; set; }

        [CustomUIView(Titolo = "Data inizio validità", DateTimeView = true)]
        public DateTime DataInizioValidita { get; set; }
        [CustomUIView(Titolo = "Data fine validità", DateTimeView = true)]
        public DateTime DataFineValidita { get; set; }
        [CustomUIView(Titolo = "Variazione %", Percentuale = true)]
        public int PercentualeVariazione { get; set; }
    }
}