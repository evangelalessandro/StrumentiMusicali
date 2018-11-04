using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class Deposito : BaseEntity
	{ 

		public string NomeDeposito { get; set; }
	}
}