using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class Cliente : BaseEntity
	{
		[CustomUIViewAttribute(Width = 100)]
		public string PIVA { get; set; }
		public string RagioneSociale { get; set; }

		public string Via { get; set; }
		public string Citta { get; set; }

		[CustomUIViewAttribute( Width = 80)]
		public int Cap { get; set; }
		public string Telefono { get; set; }
		public string Fax { get; set; }
		public string Cellulare { get; set; }
		public string LuogoDestinazione { get; set; }
		[CustomUIViewAttribute(Width = 120)]
		public string NomeBanca { get; set; }
		[CustomUIViewAttribute(Width = 80)]
		public int BancaAbi { get; set; }
		[CustomUIViewAttribute(Width = 80)]
		public int BancaCab { get; set; }
		[CustomHideUI()]
		public virtual int CodiceClienteOld { get; set; }
	}
}