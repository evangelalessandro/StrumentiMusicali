using StrumentiMusicali.Library.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Core.Attributes
{
    public class CustomFattureAttribute : Attribute
    {
        public CustomFattureAttribute()
        {

        } 
        public EnTipoDocumento TipoDocShowOnly{ get; set; }

    }
}
