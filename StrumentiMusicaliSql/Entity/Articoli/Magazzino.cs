using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public class Magazzino : BaseEntity
    {

        [Required]
        public int ArticoloID { get; set; }

        public virtual Articolo Articolo { get; set; }

        [Required]
        public int DepositoID { get; set; }

        public virtual Deposito Deposito { get; set; }

        [Required]
        public int Qta { get; set; }

        public decimal PrezzoAcquisto { get; set; }

        [MaxLength(50)]
        public string Note { get; set; } = "";

        public bool OperazioneWeb { get; set; } = false;
    }
}