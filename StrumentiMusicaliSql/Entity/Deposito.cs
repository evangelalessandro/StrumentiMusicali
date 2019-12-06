using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
    public class Deposito : BaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20, ErrorMessage = "Il nome deve essere univoco")]
        public string NomeDeposito { get; set; }


        public bool Principale { get; set; }
    }
}