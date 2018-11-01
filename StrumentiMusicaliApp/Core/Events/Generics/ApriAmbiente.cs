using StrumentiMusicali.App.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	class ApriAmbiente
	{
		public ApriAmbiente(enAmbienti tipoEnviroment)
		{
			TipoEnviroment = tipoEnviroment;
		}
		public enAmbienti TipoEnviroment { get; private set; }
	}
	 
}
