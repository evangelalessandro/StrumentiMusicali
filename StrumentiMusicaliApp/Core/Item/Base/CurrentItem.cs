using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Item.Base
{
	 
	public class CurrentItem<TBaseItem, TEntity>
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		public CurrentItem(TBaseItem itemSelected)
		{
			ItemSelected = itemSelected;
		}
		public TBaseItem ItemSelected { get; private set; }
	}
}
