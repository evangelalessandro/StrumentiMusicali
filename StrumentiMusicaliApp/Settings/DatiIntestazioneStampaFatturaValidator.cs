using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.View.Enums;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Settings
{
	public class DatiIntestazioneStampaFatturaValidator
	{
		 

		public static bool Validate()
		{
			var datiIntestazioneStampaFattura = ReadSetting();
			var list = UtilityView.GetProperties(datiIntestazioneStampaFattura);
			foreach (var item in list)
			{
				if (item.GetValue(datiIntestazioneStampaFattura) ==null || string.IsNullOrEmpty( (string) item.GetValue(datiIntestazioneStampaFattura).ToString()))
				{
					var act = new Action(() => {
						EventAggregator.Instance().Publish(
						new ApriAmbiente(enAmbiente.SettingStampa));
											});
					MessageManager.NotificaWarnig("Non sono state compilate tutte le informazioni sul negozio da stampare sulla fattura. Clicca per compilare.",act);
					
					return false;

				}
			}
			 
			 
			return true;

		}
		public static DatiIntestazioneStampaFattura ReadSetting()
		{
			using (var uof = new UnitOfWork())
			{
				var setting = uof.DatiIntestazioneStampaFatturaRepository.Find(a => 1 == 1).FirstOrDefault();
				if (setting == null)
				{
					setting = new DatiIntestazioneStampaFattura();
					uof.DatiIntestazioneStampaFatturaRepository.Add(setting);
					uof.Commit();
				}
				return setting;
			}
		}
	}
}
