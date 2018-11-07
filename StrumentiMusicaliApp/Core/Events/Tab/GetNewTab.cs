using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Enums;
using StrumentiMusicali.App.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Events.Tab
{
	public class GetNewTab
	{
		public GetNewTab(string text,enAmbiente ambiente, ICloseSave closeEvent)
		{
			Text = text;
			Ambiente = ambiente;
			CloseEvent = closeEvent;
		}
		public ICloseSave CloseEvent { get; set; }
		public string Text { get; set; }
		public enAmbiente Ambiente { get; set; }

		public TabPage Tab { get; set; }
	}
}
