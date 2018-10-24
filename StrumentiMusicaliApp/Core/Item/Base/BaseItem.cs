using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Item.Base
{
	public class BaseItem<TEntity> : BaseItemID
		where TEntity : BaseEntity
	{

		public TEntity Entity { get; set; }
	}
}
