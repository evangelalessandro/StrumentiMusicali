using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Image;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
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
		Subscription<ImageOrderSet> _subOrderImage;
		Subscription<ImageAddFiles> _subAddImage;
		Subscription<ImageRemove> _subRemoveImage;
		public ControllerImmagini() : base()
		{
			_subOrderImage =	EventAggregator.Instance().Subscribe<ImageOrderSet>(OrderImage);
			_subAddImage = EventAggregator.Instance().Subscribe<ImageAddFiles>(AddImageFiles);
			_subRemoveImage = EventAggregator.Instance().Subscribe<ImageRemove>(RemoveImage);
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
		public new void Dispose()
		{
			base.Dispose();
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void RemoveImage(ImageRemove obj)
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
						var articolo = uof.ArticoliRepository
								.Find(a => a.ID == obj.FotoArticolo.ArticoloID).First();
						/*se cambio immagini devo aggiornare le immagini su, quindi aggiorno il flag*/
						if (!articolo.ImmaginiDaCaricare)
						{
							articolo.ImmaginiDaCaricare = true;
							uof.ArticoliRepository.Update(articolo);
						}
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

		private void OrderImage(ImageOrderSet obj)
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
		private void AddImageFiles(ImageAddFiles args)
		{
			if (!CheckFolderImmagini())
				return;
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
					var articolo= uof.ArticoliRepository
						.Find(a => a.ID == args.Articolo.ID).First();
					/*se cambio immagini devo aggiornare le immagini su, quindi aggiorno il flag*/
					if (!articolo.ImmaginiDaCaricare )
					{ 
						articolo.ImmaginiDaCaricare = true;
						uof.ArticoliRepository.Update(articolo);
					}
					if (save.SaveEntity(string.Format(@"{0} Immagine\i aggiunta\e", args.Files.Count())))
					{
						EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
	}
}