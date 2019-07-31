﻿using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Settings
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
        public static bool ScadutoTempoBackup()
        {
            var setting = SettingBackupFtpValidator.ReadSetting();
            if (setting.ControllaEsecuzioneBackup > 0)
            {
                if (setting.UltimoBackup.Date < DateTime.Now.AddDays(-setting.ControllaEsecuzioneBackup).Date)
                {
                    return true;
                }
            }
            return false;
        }

    }
}