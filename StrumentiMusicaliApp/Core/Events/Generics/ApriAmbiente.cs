using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	class ApriAmbiente
	{
		public ApriAmbiente(enTipoEnviroment tipoEnviroment)
		{
			TipoEnviroment = tipoEnviroment;
		}
		public enTipoEnviroment TipoEnviroment { get; private set; }
	}
	public enum enTipoEnviroment
	{
		Fatturazione,
		SettingFatture,
		SettingUrl,
		LogView,

	}
}
