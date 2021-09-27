using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using StrumentiMusicali.Library.Entity.RegistratoreDiCassa;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    public class Categoria : BaseEntity
    {

        [MaxLength(50)]
        public string Reparto { get; set; }
        [MaxLength(50)]
        public string Nome { get; set; }
        public string CategoriaCondivisaCon { get; set; }
        public int Codice { get; set; }



        [CustomUIView(Width = 350, Ordine = 1, Combo = TipoDatiCollegati.GruppoCodiceRegCassa, Titolo = "Gruppo Codice Registratore di Cassa",
            Category = "Registratore di cassa")]
        public int? GruppoCodiceRegCassaID { get; set; }

        [CustomHideUI]
        public virtual GruppoCodiceRegCassa GruppoCodiceRegCassa { get; set; }


        [CustomUIView(Width = 80, Titolo = "Maggiorazione percentuale da prezzo acquisto a prezzo web", Ordine = 30,
            Category = "Prezzo")]
        public int PercMaggNegozioRispettoWeb { get; set; } = 10;

        [CustomUIView(Width = 80, Titolo = "Maggiorazione percentuale da prezzo negozio, se acquisto =0, a prezzo web", Ordine = 30,
            Category = "Prezzo")]
        public int PercMaggNegozioRispettoWebSeAqtZero { get; set; } = 10;

        [CustomUIView(Width = 80, Titolo = "Maggiorazione da prezzo web a prezzo negozio", Ordine = 40,
            Category = "Prezzo")]
        public int PercMaggDaWebaNegozio { get; set; } = 20;
    }
}