using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Entity.Articoli;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Utility
{
    public static class ImageArticoloSave
    {
        /// <summary>
        /// Controllo la cartella locale per le immagini se è correttamente settata e attiva
        /// </summary>
        /// <returns></returns>
        private static bool CheckFolderImmagini()
        {
            return SettingSitoValidator.CheckFolderImmagini();
        }
        public static bool AddImageFiles(ImageArticoloAddFiles args)
        {

            if (!CheckFolderImmagini())
                return false;
            try
            {
                var folderFoto = SettingSitoValidator.ReadSetting().CartellaLocaleImmagini;
                using (var save = new SaveEntityManager())
                {
                    var uof = save.UnitOfWork;
                    var maxOrdineItem = uof.FotoArticoloRepository
                        .Find(a => a.ArticoloID == args.Articolo.ID)
                        .OrderByDescending(a => a.Ordine).FirstOrDefault();

                    var maxOrdine = 0;
                    if (maxOrdineItem != null)
                    {
                        maxOrdine = maxOrdineItem.Ordine + 1;
                    }

                    foreach (var item in args.Files)
                    {
                        var file = new FileInfo(item);

                        var newName = DateTime.Now.Ticks.ToString() + file.Extension;
                        File.Copy(item, Path.Combine(folderFoto, newName));

                        uof.FotoArticoloRepository.Add(
                            new FotoArticolo()
                            {
                                ArticoloID = args.Articolo.ID,
                                UrlFoto = newName,
                                Ordine = maxOrdine
                            });
                        maxOrdine++;
                    }
                    
                    if (save.SaveEntity(string.Format(@"{0} Immagine\i aggiunta\e", args.Files.Count())))
                    {
                        EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
            return false;
        }
    }
}
