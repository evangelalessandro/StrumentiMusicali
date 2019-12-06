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
    }
}