using StrumentiMusicali.Library.Core.Attributes;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Base;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Utility
{
    public class EvaluateEnableField
    {
        public bool VisibleItem(object Item, string nameProp)
        {
            var prop = UtilityProp.GetProperties(Item).Where(a => a.Name == nameProp).First();

            var sel = (CustomFattureAttribute)prop.GetCustomAttributes(typeof(CustomFattureAttribute), true).FirstOrDefault();

            if (sel == null)
                return true;

            if (Item is FatturaRiga)
                if (((FatturaRiga)Item).Fattura.TipoDocumento == sel.TipoDocShowOnly)
                    return true;

            if (Item is FatturaRigaItem)
                if (((FatturaRigaItem)Item).TipoDoc == sel.TipoDocShowOnly)
                    return true;

            return false;

        }
    }
}
