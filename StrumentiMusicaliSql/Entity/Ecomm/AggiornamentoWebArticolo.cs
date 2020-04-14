using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Articoli;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Ecomm
{
    public class AggiornamentoWebArticolo : Base.BaseEntity
    {
        public int ArticoloID { get; set; }

        public virtual Articolo Articolo { get; set; }

        [CustomUIView(Width = 80, Enable = false, Titolo = "Data aggiornamento giacenza locale", Category = "Interni", DateTimeView = true)]
        public DateTime DataUltimoAggMagazzino { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(Width = 80, Enable = false, Titolo = "Data aggiornamento giacenza Web", Category = "Interni", DateTimeView = true)]
        public DateTime DataUltimoAggMagazzinoWeb { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(Width = 80, Enable = false, Titolo = "Data aggiornamento foto locale", Category = "Interni", DateTimeView = true)]
        public DateTime DataUltimoAggFoto { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(Ordine = 10, Enable = false, Category = "Allineamento")]
        [MaxLength(50)]
        public string CodiceArticoloEcommerce { get; set; } = "";

        [CustomUIView(Ordine = 20, Enable = false, DateTimeView = true, Category = "Allineamento")]
        public DateTime DataUltimoAggiornamentoWeb { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(Width = 80, Enable = false, Titolo = "Data aggiornamento foto Web", Category = "Interni", DateTimeView = true)]
        public DateTime DataUltimoAggFotoWeb { get; set; } = new DateTime(1900, 1, 1);
    }
}