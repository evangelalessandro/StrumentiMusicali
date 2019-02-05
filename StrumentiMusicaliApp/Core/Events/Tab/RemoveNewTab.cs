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
		public RemoveNewTab(DevExpress.XtraTab.XtraTabPage tab)
		{
			Tab = tab;
		}
		
		public DevExpress.XtraTab.XtraTabPage Tab { get; set; }
	}
}
