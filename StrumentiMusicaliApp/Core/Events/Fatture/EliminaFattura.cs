using StrumentiMusicali.App.Core.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Fatture
{
	class EliminaFattura : FatturaCurrent
	{
		public EliminaFattura(FatturaItem item)
			: base(item)
		{

		}
	}
	 
}
