using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class Utente : BaseEntity
	{
		[Required]
		public string NomeUtente { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public bool AdminUtenti { get; set; }

		public bool Fatturazione { get; set; }
		[CustomHideUIAttribute]
		public bool ScontaArticoli { get; set; }

		public bool Magazzino { get; set; }

	}
}
