using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core.Events
{
	public class ArticoloCurrent
	{
		public ArticoloCurrent(ArticoloItem itemSelected)
		{
			ItemSelected = itemSelected;
		}
		public ArticoloItem ItemSelected { get; private set; }
	}
}
