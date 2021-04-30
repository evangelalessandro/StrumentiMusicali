using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.Core.Settings
{
    public static class SettingScontrinoValidator
    {


        /// <summary>
        /// Controllo la cartella locale se è correttamente settata e attiva
        /// </summary>
        /// <returns></returns>
        public static bool Check()
        {
            var setting = ReadSetting();
            var item = setting.FolderDestinazione;
            var act = new Action(() => EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingScontrino)));

            if (string.IsNullOrEmpty(item))
            {
                MessageManager.NotificaWarnig("Occorre impostare la cartella di destinazione! " + Environment.NewLine + "Clicca per settare.", act);
            
                return false;
            }
            if (!Directory.Exists(item))
            {
                MessageManager.NotificaWarnig(string.Format("La cartella di destinazione specificata {0} non esiste o non è raggiungibile!"
                    + Environment.NewLine + "Clicca per settare.", item), act);
                return false;
            }
            return true;
        }


        public static SettingScontrino ReadSetting()
        {
            using (var uof = new UnitOfWork())
            {
                var setting = uof.SettingScontrino.Find(a => 1 == 1).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingScontrino();
                    uof.SettingScontrino.Add(setting);
                    uof.Commit();
                }
                return setting;
            }
        }
    }
}
