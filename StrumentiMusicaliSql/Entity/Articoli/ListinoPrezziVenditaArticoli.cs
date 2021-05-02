using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    [Table("ListinoPrezzi_Articoli")]
    public class ListinoPrezziVenditaArticoli : BaseEntity
    {
        public int ArticoloID { get; set; }

        public virtual Articolo Articolo { get; set; }


        public int ListinoPrezziVenditaNomeID { get; set; }

        public virtual ListinoPrezziVenditaNome ListinoPrezziVenditaNome { get; set; }

        ///// <summary>
        ///// Valore di base, ci deve sempre essere
        ///// </summary>
        //public bool Principale { get; set; }
        [CustomUIView(Width = 100, Titolo = "Prezzo Imponibile")]
        public decimal? Imponibile { get; set; }


        [CustomUIView(Width = 100, Titolo = "Prezzo ivato")]
        public decimal? Ivato { get; set; }

        [CustomUIView(Width = 100, Titolo = "Ricarico % su prezzo acquisto")]
        public int? RicaricoPercSuPrezzoAcquisto { get; set; }

        [CustomUIView(Width = 100, Titolo = "Sconto 1 %")]
        public int? Sconto1 { get; set; }

        [CustomUIView(Width = 100, Titolo = "Sconto 2 %")]
        public int? Sconto2 { get; set; }


        [CustomUIView(Width = 100, Titolo = "Sconto 3 %")]
        public int? Sconto3 { get; set; }
    }
}