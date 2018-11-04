using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class RigaDoc : BaseEntity
	{
		
		[CustomUIViewAttribute(Ordine = 1)]
		public string CodiceArticoloOld { get; set; }

		[ComboData(TypeList = "Articoli")]
		[CustomHideUIAttribute]
		public int? ArticoloID { get; set; }

		[CustomHideUIAttribute]
		public virtual Articolo Articolo { get; set; }


		[CustomUIViewAttribute(Ordine = 2,Width = 350)]
		public string Descrizione { get; set; }

		[CustomUIViewAttribute(Width =50, Ordine =	 3)]
		public int Qta { get; set; }

		[CustomHideUIAttribute]
		public int OrdineVisualizzazione { get; set; }
	}
}