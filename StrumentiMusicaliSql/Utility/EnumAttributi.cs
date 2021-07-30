using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Utility
{
    public class EnumAttributi
    {
        public static T GetAttribute<T>(Enum enumValue) where T : Attribute
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException();
            }
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            return (T)member?.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
    }
}
