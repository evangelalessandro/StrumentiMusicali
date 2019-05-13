using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Item
{
	public class FatturaRigaItem : BaseItem<FatturaRiga>
	{
		public string CodiceArt { get; set; }

		public string RigaDescrizione { get; set; }

		public int RigaQta { get; set; }

		public decimal PrezzoUnitario { get; set; }

		public decimal RigaImporto { get; set; }

		public string Iva { get; set; }
	}
}