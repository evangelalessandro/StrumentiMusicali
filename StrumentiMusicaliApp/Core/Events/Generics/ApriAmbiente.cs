using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Generics
{
	public class ApriAmbiente
	{
		public ApriAmbiente(enAmbiente tipoEnviroment)
		{
			TipoEnviroment = tipoEnviroment;
		}
		public enAmbiente TipoEnviroment { get; private set; }
	}
	 
}
