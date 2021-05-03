using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Articoli
{
 

    public class Categoria : BaseEntity
    {
        [MaxLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Specificare reparto")]
        public string Reparto { get; set; }
        [MaxLength(50)]

        [Required(AllowEmptyStrings = false, ErrorMessage = "Specificare nome categoria")]
        public string Nome { get; set; }
        [MaxLength(250)]
        [CustomUIView(Titolo = "Nomi categorie con cui viene condivisa la categoria")]

        public string CategoriaCondivisaCon { get; set; } = "";

        public int Codice { get; set; } = 1000;
         

        [CustomUIView(Width = 80, Titolo = "Maggiorazione da prezzo web a prezzo negozio, Import da web", Ordine = 40,
            Category = "Prezzo")]
        public int PercMaggDaWebaNegozio { get; set; } = 20;
    }
}