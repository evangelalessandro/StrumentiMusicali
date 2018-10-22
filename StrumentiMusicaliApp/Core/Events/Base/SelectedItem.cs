using StrumentiMusicali.App.Core.Events.Articoli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Base
{
	class SelectedItem
	{
		public SelectedItem(Item.Base.BaseItem item)
		{
			if (item is ArticoloItem)
			{
				new ArticoloSelected(item as ArticoloItem);
			}
		}
	}
}
