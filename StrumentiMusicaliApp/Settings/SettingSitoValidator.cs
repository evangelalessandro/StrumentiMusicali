using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.App.Settings
{
    public static class SettingSitoValidator
    {


        /// <summary>
        /// Controllo la cartella locale per le immagini se è correttamente settata e attiva
        /// </summary>
        /// <returns></returns>
        public static bool CheckFolderImmagini()
        {
            var settingSito = ReadSetting();
            var item = settingSito.CartellaLocaleImmagini;
            var act = new Action(() => EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingSito)));
            if (string.IsNullOrEmpty(item))
            {
                MessageManager.NotificaWarnig("Occorre impostare la cartella per le immagini in locale! " + Environment.NewLine + "Clicca per settare.", act);
                return false;
            }
            if (!Directory.Exists(item))
            {
                MessageManager.NotificaWarnig(string.Format("La cartella per le immagini in locale specificata {0} non esiste o non è raggiungibile!"
                    + Environment.NewLine + "Clicca per settare.", item), act);
                return false;
            }
            return true;
        }
      
      
        public static SettingSito ReadSetting()
        {
            using (var uof = new UnitOfWork())
            {
                var setting = uof.SettingSitoRepository.Find(a => 1 == 1).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingSito();
                    uof.SettingSitoRepository.Add(setting);
                    uof.Commit();
                }
                return setting;
            }
        }
    }
}
