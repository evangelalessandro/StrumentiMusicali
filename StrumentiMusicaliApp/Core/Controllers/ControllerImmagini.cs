using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
    internal class ControllerImmagini : BaseController, IDisposable
    {
        Subscription<ImageArticoloOrderSet> _subOrderImage;
        Subscription<ImageArticoloAddFiles> _subAddImage;
        Subscription<ImageArticoloRemove> _subRemoveImage;
        public ControllerImmagini() : base()
        {
            _subOrderImage = EventAggregator.Instance().Subscribe<ImageArticoloOrderSet>(OrderImage);
            _subAddImage = EventAggregator.Instance().Subscribe<ImageArticoloAddFiles>(AddImageFiles);
            _subRemoveImage = EventAggregator.Instance().Subscribe<ImageArticoloRemove>(RemoveImage);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerImmagini()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected new void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                EventAggregator.Instance().UnSbscribe(_subOrderImage);
                EventAggregator.Instance().UnSbscribe(_subAddImage);
                EventAggregator.Instance().UnSbscribe(_subRemoveImage);
            }
            // free native resources if there are any.
        }
        public override void Dispose()
        {
            base.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void RemoveImage(ImageArticoloRemove obj)
        {

            if (!CheckFolderImmagini())
                return;
            var folderFoto = SettingSitoValidator.ReadSetting().CartellaLocaleImmagini;
            try
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare l'immagine selezionata?"))
                    return;
                var listFileToDelete = new List<string>();
                using (var curs = new CursorManager())
                {
                    using (var uof = new UnitOfWork())
                    {
                        var item = uof.FotoArticoloRepository.Find(
                           a => a.ID == obj.FotoArticolo.ID).FirstOrDefault();
                        if (item == null)
                        {
                            return;
                        }
                        RimuoviItemDaRepo(folderFoto, listFileToDelete, uof, item);
                         
                        uof.Commit();
                    }
                }
                EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
                DeleteFile(listFileToDelete);
                MessageManager.NotificaInfo("Eliminazione avvenuta con successo");
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        public void DeleteFile(List<string> listFileToDelete)
        {
            Application.DoEvents();
            Thread.Sleep(1000);

            foreach (var item in listFileToDelete)
            {
                Application.DoEvents();
                Thread.Sleep(100);
                File.Delete(item);
            }
        }

        public void RimuoviItemDaRepo(string folderFoto, List<string> listFileToDelete, UnitOfWork uof, FotoArticolo item)
        {
            uof.FotoArticoloRepository.Delete(item);
            listFileToDelete.Add(Path.Combine(folderFoto, item.UrlFoto));
        }

        private void OrderImage(ImageArticoloOrderSet obj)
        {

            try
            {
                using (var curs = new CursorManager())
                {
                    using (var save = new SaveEntityManager())
                    {
                        var uof = save.UnitOfWork;
                        var articolo = uof.FotoArticoloRepository.Find(
                            a => a.ID == obj.FotoArticolo.ID).Select(a => a.Articolo).FirstOrDefault();
                        var list = uof.FotoArticoloRepository.Find(
                            a => a.Articolo.ID == articolo.ID).OrderBy(a => a.Ordine).ToList();
                        foreach (var item in list)
                        {
                            if (item.ID == obj.FotoArticolo.ID)
                            {
                                switch (obj.TipoOperazione)
                                {
                                    case enOperationOrder.ImpostaPrincipale:
                                        item.Ordine = -1;
                                        break;

                                    case enOperationOrder.AumentaPriorita:
                                        var itemToUpdate = list.Where(a => a.Ordine == item.Ordine - 1).FirstOrDefault();
                                        if (itemToUpdate != null)
                                        {
                                            itemToUpdate.Ordine++;
                                        }
                                        item.Ordine--;
                                        break;

                                    case enOperationOrder.DiminuisciPriorita:
                                        var itemToUpdateTwo = list.Where(a => a.Ordine == item.Ordine + 1).FirstOrDefault();
                                        if (itemToUpdateTwo != null)
                                        {
                                            itemToUpdateTwo.Ordine--;
                                        }
                                        item.Ordine++;
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        var setOrdine = 0;
                        foreach (var item in list.OrderBy(a => a.Ordine))
                        {
                            item.Ordine = setOrdine;
                            setOrdine++;
                            uof.FotoArticoloRepository.Update(item);
                        }

                        if (save.SaveEntity(enSaveOperation.OpSave))
                        {
                            EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        /// <summary>
        /// Controllo la cartella locale per le immagini se è correttamente settata e attiva
        /// </summary>
        /// <returns></returns>
        public bool CheckFolderImmagini()
        {
            return SettingSitoValidator.CheckFolderImmagini();
        }
        private void AddImageFiles(ImageArticoloAddFiles args)
        {
            ImageArticoloSave.AddImageFiles(args);
        }
    }
}