using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Utility
{
    public static class UtilityProp
    {
        public static IEnumerable<PropertyInfo> GetProperties(object obj)
        {
            Type t = obj.GetType();

            return t.GetProperties()
                .Where(p => p.Name != "EntityKey" && p.Name != "EntityState")
                .Select(p => p).ToList();
        }
    }
}
