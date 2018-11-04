using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Settings
{
	public class DatiIntestazioneStampaFattura
	{
		public string NegozioRagSoc { get; set; } = "";
		public string NegozioIndirizzo { get; set; } = "";
		public string NegozioTelefonoFax { get; set; } = "";
		public string NegozioPIVA { get; set; } = "";
		public string NegozioCF { get; set; } = "";
		public string NegozioEmail { get; set; } = "";
		public string NomeBanca { get; set; } = "";
		public string IBAN { get; set; } = "";

		public bool Validate()
		{
			var list = UtilityView.GetProperties(this);
			foreach (var item in list)
			{
				if (string.IsNullOrEmpty( (string) item.GetValue(this)))
				{
					var act = new Action(() => {
						EventAggregator.Instance().Publish(
						new ApriAmbiente(enAmbienti.SettingStampa));
											});
					MessageManager.NotificaWarnig("Non sono state compilate tutte le informazioni sul negozio da stampare sulla fattura. Clicca per compilare.",act);
					
					return false;

				}
			}
			 
			 
			return true;

		}
	}
}
