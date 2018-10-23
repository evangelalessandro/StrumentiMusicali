using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class Cliente : BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		public string PIVA { get; set; }
		public string RagioneSociale { get; set; }

		public string Via { get; set; }
		public string Citta { get; set; }
		public int Cap { get; set; }
		public string Telefono { get; set; }
		public string Fax { get; set; }
		public string Cellulare { get; set; }
		public string LuogoDestinazione { get; set; }
		public string NomeBanca { get; set; }
		public int BancaAbi { get; set; }
		public int BancaCab { get; set; }

		public virtual int CodiceClienteOld { get; set; }
	}
}