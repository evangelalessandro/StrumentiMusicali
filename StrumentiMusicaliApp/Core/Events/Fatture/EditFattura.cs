using StrumentiMusicali.App.Core.Item;

namespace StrumentiMusicali.App.Core.Events.Fatture
{
	internal class EditFattura : FatturaCurrent
	{
		public EditFattura(FatturaItem item)
			: base(item)
		{
		}
	}
}