using StrumentiMusicali.App.Core.Item;

namespace StrumentiMusicali.App.Core.Events.Fatture
{
	internal class EliminaFattura : FatturaCurrent
	{
		public EliminaFattura(FatturaItem item)
			: base(item)
		{
		}
	}
}