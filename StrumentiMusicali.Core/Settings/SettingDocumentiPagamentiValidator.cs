using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.App.Settings
{
    public static class SettingDocumentiPagamentiValidator
    {
        public static bool CheckFolderPdfPagamenti()
        {
            var settingSito = ReadSetting();
            var item = settingSito.CartellaReteDocumentiPagamenti;
            var act = new Action(() => EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingDocPagamenti)));
            if (string.IsNullOrEmpty(item))
            {
                MessageManager.NotificaWarnig("Occorre impostare la cartella di rete per i documenti dei pagamenti!" + Environment.NewLine + "Clicca per settare.", act);
                return false;
            }
            if (!Directory.Exists(item))
            {
                MessageManager.NotificaWarnig(string.Format("La cartella per cartella di rete per i documenti dei pagamenti specificata {0} non esiste o non è raggiungibile!"
                    + Environment.NewLine + "Clicca per settare.", item), act);
                return false;
            }
            return true;
        }
        public static SettingDocumentiPagamenti ReadSetting()
        {
            using (var uof = new UnitOfWork())
            {
                var setting = uof.SettingDocumentiPagamentiRepository.Find(a => 1 == 1).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingDocumentiPagamenti();
                    uof.SettingDocumentiPagamentiRepository.Add(setting);
                    uof.Commit();
                }
                return setting;
            }
        }
    }
}
