using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Generics
{ 
	public class Save<TBaseItem, TEntity>
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		public Save()
		{
		}
	}
}
