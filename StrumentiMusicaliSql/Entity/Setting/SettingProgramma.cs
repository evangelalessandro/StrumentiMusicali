using StrumentiMusicali.Library.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Setting
{
    [Table(name:"SettingProgramma")]
    public class SettingProgramma : Base.BaseEntity
    {

        public bool GestioneLibri { get; set; } = false;

        [StringLength(50,MinimumLength =1)]
        [CustomUIView(Ordine = 1, Titolo = "Nome anagrafica (es. 'Articoli' o 'Strumenti musicali')")]
        public string NomeAnagrafica { get; set; } = "Articoli";

        public bool AmbientePagamentiRateali { get; set; } = false;
    }
}