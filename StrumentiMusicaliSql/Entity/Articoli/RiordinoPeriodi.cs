using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    public class RiordinoPeriodi : BaseEntity
    {
        [StringLength(50,ErrorMessage = "Descrizione troppo lunga")]
        public string Descrizione { get; set; }

        [CustomUIView(Ordine = 11, Titolo = "Se vero, tutto l'anno è da gestire il sottoscorta e quindi riordino")]
        public bool TuttoAnno { get; set; } = false;

        [CustomUIView(Ordine = 12, Titolo = "Inizio periodo di validità di sotto scorta (dd/MM)",  DateView = true, MaskDate = "dd/MM")]
        public DateTime PeriodoSottoScortaInizio { get; set; } = new DateTime(1900, 1, 1);
        [CustomUIView(Ordine = 13, Titolo = "Fine periodo di validità di sotto scorta  (dd/MM)",  DateView = true, MaskDate = "dd/MM")]
        public DateTime PeriodoSottoScortaFine { get; set; } = new DateTime(1900, 12, 31);
    }
}
