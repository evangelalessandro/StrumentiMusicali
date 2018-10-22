using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Item.Base
{
	 
	public class CurrentItem<TbaseItem> 
		where TbaseItem : BaseItem
	{
		public CurrentItem(BaseItem itemSelected)
		{
			ItemSelected = itemSelected;
		}
		public BaseItem ItemSelected { get; private set; }
	}
}
