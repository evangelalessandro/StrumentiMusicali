using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Events.Scontrino
{
    public class ScontrinoStampa
    {
        public enTipoStampa TipoStampa { get; set; } = enTipoStampa.Generico;
    }
    public enum enTipoStampa
    {
        Generico,
        Contanti,
        Carte

    }
}
