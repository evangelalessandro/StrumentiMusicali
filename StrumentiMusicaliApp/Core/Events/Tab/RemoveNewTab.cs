using StrumentiMusicali.App.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Events.Tab
{
	public class RemoveNewTab
	{
		public RemoveNewTab(TabPage tab)
		{
			Tab = tab;
		}
		
		public TabPage Tab { get; set; }
	}
}
