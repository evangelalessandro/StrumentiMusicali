
using StrumentiMusicali.Core.Enum;

namespace StrumentiMusicali.Library.Core.Events.Generics
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
