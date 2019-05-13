using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.View.Enums;

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

		public DevExpress.XtraTab.XtraTabPage Tab { get; set; }
	}
}
