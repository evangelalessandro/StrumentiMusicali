using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public class FotoArticolo : BaseEntity
    {
        public int ArticoloID { get; set; }

        public virtual Articolo Articolo { get; set; }

        [Required]
        public string UrlFoto { get; set; }

        [Required]
        public int Ordine { get; set; } = -1;
    }
}