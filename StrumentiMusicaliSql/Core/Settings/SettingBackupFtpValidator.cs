using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System.Linq;

namespace StrumentiMusicali.Library.Core.Settings
{
    public static class SettingBackupFtpValidator
    {
        public static SettingBackupFtp ReadSetting()
        {
            using (var uof = new UnitOfWork())
            {
                var setting = uof.SettingBackupFtpRepository.Find(a => 1 == 1).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingBackupFtp();
                    uof.SettingBackupFtpRepository.Add(setting);
                    uof.Commit();
                }
                return setting;
            }
        }

    }
}
