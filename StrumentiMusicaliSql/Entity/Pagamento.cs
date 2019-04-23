using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class Pagamento : BaseEntity
	{ 
 		 
        [Required]
        public string Cognome { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public int ArticoloID { get; set; }

        public virtual Articolo Articolo { get; set; }
        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 4)]
        public decimal ImportoTotale { get; set; }

        
        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 5)]
        public DateTime DataInizio { get; set; } = DateTime.Now;
        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 9)]
        public decimal ImportoRata { get; set; }
        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 10)]
        public DateTime DataRata { get; set; }

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 11)]
        public decimal ImportoResiduo { get; set; }
    
 
	}
}