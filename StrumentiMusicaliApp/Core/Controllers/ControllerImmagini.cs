using StrumentiMusicali.App.Core.Events.Image;
using StrumentiMusicali.App.Core.Manager;
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
	class ControllerImmagini :BaseController
	{
		public ControllerImmagini() : base()
		{
			EventAggregator.Instance().Subscribe<ImageOrderSet>(OrderImage);
			EventAggregator.Instance().Subscribe<ImageAddFiles>(AddImageFiles);
			EventAggregator.Instance().Subscribe<ImageRemove>(RemoveImage);

		}

		private void RemoveImage(ImageRemove obj)
		{
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
						uof.FotoArticoloRepository.Delete(item);
						listFileToDelete.Add(Path.Combine(FolderFoto, item.UrlFoto));

						uof.Commit();
					}
				}
				EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());

				Application.DoEvents();
				Thread.Sleep(1000);

				foreach (var item in listFileToDelete)
				{
					Application.DoEvents();
					Thread.Sleep(100);
					File.Delete(item);
				}
				MessageManager.NotificaInfo("Eliminazione avvenuta con successo");
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void OrderImage(ImageOrderSet obj)
		{
			try
			{
				using (var curs = new CursorManager())
				{
					using (var uof = new UnitOfWork())
					{
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

						uof.Commit();
					}
				}
				MessageManager.NotificaInfo("Salvataggio avvenuto con successo");
				EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		public static string FolderFoto {
			get {
				return @"C:\Users\fastcode13042017\Downloads\Mercatino musicale\Immagini";
				//return Path.Combine(Application.ExecutablePath, "Foto");
			}
		}


		private void AddImageFiles(ImageAddFiles args)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
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
						File.Copy(item, Path.Combine(FolderFoto, newName));

						uof.FotoArticoloRepository.Add(
							new FotoArticolo()
							{
								ArticoloID = args.Articolo.ID
							,
								UrlFoto = newName
							,
								Ordine = maxOrdine
							});
						maxOrdine++;
					}
					uof.Commit();
				}
				EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
				MessageManager.NotificaInfo(string.Format(@"{0} Immagine\i aggiunta\e", args.Files.Count()));
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

	}
}
