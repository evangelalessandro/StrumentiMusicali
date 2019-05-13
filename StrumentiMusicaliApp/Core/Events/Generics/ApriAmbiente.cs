using StrumentiMusicali.Library.View.Enums;

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
