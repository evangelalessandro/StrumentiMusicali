using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using System;

namespace StrumentiMusicali.Library.Core.Item
{
    public class RiordinoPeriodiItem : BaseItem<RiordinoPeriodi>
    {
        public RiordinoPeriodiItem()
        {

        }
        public RiordinoPeriodiItem(RiordinoPeriodi item)
        {
            Descrizione = item.Descrizione;
            TuttoAnno = item.TuttoAnno;
            PeriodoSottoScortaFine = item.PeriodoSottoScortaFine;
            PeriodoSottoScortaInizio = item.PeriodoSottoScortaInizio;
            Entity = item;
            ID = item.ID;
        }
        public string Descrizione { get; set; }

        
        public bool TuttoAnno { get; set; } = false;

        [CustomUIView(Ordine = 11, Titolo = "Inizio periodo", DateView = true, MaskDate = "dd/MM")]
        public DateTime PeriodoSottoScortaInizio { get; set; } = new DateTime(1900, 1, 1);
        [CustomUIView(Ordine = 11, Titolo = "Fine periodo ", DateView = true, MaskDate = "dd/MM")]
        public DateTime PeriodoSottoScortaFine { get; set; } = new DateTime(1900, 12, 31);
    }
}
