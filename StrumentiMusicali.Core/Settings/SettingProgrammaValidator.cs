using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Settings
{
    public static class  SettingProgrammaValidator
    {
        public static SettingProgramma ReadSetting()
    {
        using (var uof = new UnitOfWork())
        {
            var setting = uof.SettingProgrammaRepository.Find(a => 1 == 1).FirstOrDefault();
            if (setting == null)
            {
                setting = new SettingProgramma();
                uof.SettingProgrammaRepository.Add(setting);
                uof.Commit();
            }
            return setting;
        }
    }
}
}
