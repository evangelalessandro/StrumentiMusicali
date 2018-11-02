using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Events.Image;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Forms;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerArticoli : BaseControllerGeneric<Articolo,
		ArticoloItem>, IDisposable
	{
		//private List<Subscription<object>> subList = new List<Subscription<object>>();
		public ControllerArticoli()
			: base(enAmbienti.ArticoliList, enAmbienti.Articolo)
		{
			
		    EventAggregator.Instance().Subscribe<Add<Articolo>>(AggiungiArticolo);
			EventAggregator.Instance().Subscribe<Edit<Articolo>>(EditArt);


			EventAggregator.Instance().Subscribe<Remove<Articolo>>(DeleteArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDuplicate>(DuplicaArticolo);

			EventAggregator.Instance().Subscribe<ImageAdd>(AggiungiImmagine);
			EventAggregator.Instance().Subscribe<ImportArticoliCSVMercatino>(ImportaCsvArticoli);
			EventAggregator.Instance().Subscribe<InvioArticoliCSV>(InvioArticoli);
			EventAggregator.Instance().Subscribe<Save<Articolo>>(SaveArticolo);


			AggiungiComandi();
		}
		public new void Dispose()
		{
			 
		}

		private void AggiungiComandi()
		{
			var pnl = GetMenu().Tabs[0].Pannelli.Last();
			var rib1 = pnl.Add("Duplica", Properties.Resources.Duplicate, true);
			rib1.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish<ArticoloDuplicate>(new ArticoloDuplicate());

			};
		}
		private void InvioArticoli(InvioArticoliCSV obj)
		{
			using (var export = new Exports.ExportArticoliCsv())
			{
				export.InvioArticoli();
			}
		}

		~ControllerArticoli()
		{
			var dato = this.ReadSetting(Settings.enAmbienti.ArticoliList);
			if (SelectedItem != null )
			{
				dato.LastItemSelected = SelectedItem.ID;
				this.SaveSetting(Settings.enAmbienti.ArticoliList, dato);
			}
		}

		  

		private void EditArt(Edit<Articolo> obj)
		{
			var item = ReadSetting().settingSito;
			if (!item.CheckFolderImmagini())
			{
				return;
			}
			EditItem = SelectedItem;
			using (var view = new DettaglioArticoloView(this, item))
			{
				ShowView(view, Settings.enAmbienti.Articolo);
			}
		}
		private void AggiungiArticolo(object articoloAdd)
		{
			_logger.Info("Apertura ambiente AggiungiArticolo");
			var item = ReadSetting().settingSito;
			if (!item.CheckFolderImmagini())
			{
				return;
			}
			EditItem = new Articolo();
			using (var view = new DettaglioArticoloView(this, item))
			{
				ShowView(view, Settings.enAmbienti.Articolo);
			}
		}

		private void DuplicaArticolo(ArticoloDuplicate obj)
		{
			try
			{
				using (var saveEntity = new SaveEntityManager())
				{
					var uof = saveEntity.UnitOfWork;
					var itemCurrent = (SelectedItem);
					var art=
					(new StrumentiMusicali.Library.Entity.Articolo()
					{
						Categoria = itemCurrent.Categoria,
						Condizione = itemCurrent.Condizione,
						BoxProposte = itemCurrent.BoxProposte,
						Marca = itemCurrent.Marca,
						Prezzo = itemCurrent.Prezzo,
						PrezzoARichiesta = itemCurrent.PrezzoARichiesta,
						PrezzoBarrato = itemCurrent.PrezzoBarrato,
						Testo = itemCurrent.Testo,
						Titolo = "* " + itemCurrent.Titolo,
						UsaAnnuncioTurbo = itemCurrent.UsaAnnuncioTurbo,
						DataUltimaModifica = DateTime.Now,
						DataCreazione = DateTime.Now
					});
					uof.ArticoliRepository.Add(art);
					if (saveEntity.SaveEntity(enSaveOperation.Duplicazione))
					{
						_logger.Info("Duplica articolo");

						SelectedItem = art;
						EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>());

						 
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void ImportaCsvArticoli(ImportArticoliCSVMercatino obj)
		{
			try
			{
				using (OpenFileDialog res = new OpenFileDialog())
				{
					res.Title = "Seleziona file da importare";
					//Filter
					res.Filter = "File csv|*.csv;";

					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						//Get the file's path
						var fileName = res.FileName;

						using (var curs = new CursorManager())
						{
							using (StreamReader sr = new StreamReader(fileName, Encoding.Default, true))
							{
								String line;
								bool firstLine = true;
								int progress = 1;
								// Read and display lines from the file until the end of
								// the file is reached.
								using (var uof = new UnitOfWork())
								{
									while ((line = sr.ReadLine()) != null)
									{
										if (!firstLine)
										{
											progress = ImportLine(line, progress, uof);
										}
										firstLine = false;
									}
									uof.Commit();
								}
							}
						}
						EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>());
						MessageManager.NotificaInfo("Terminata importazione articoli");
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private int ImportLine(string line, int progress, UnitOfWork uof)
		{
			var dat = line.Split('§');
			var cond = enCondizioneArticolo.Nuovo;
			if (dat[2] == "N")
			{
				cond = enCondizioneArticolo.Nuovo;
			}
			else if (dat[2] == "U")
			{
				cond = enCondizioneArticolo.UsatoGarantito;
			}
			else if (dat[2] == "E")
			{
				cond = enCondizioneArticolo.ExDemo;
			}
			else
			{
				throw new Exception("Tipo dato non gestito o mancante nella condizione articolo.");
			}
			decimal prezzo = 0;
			decimal prezzoBarrato = 0;
			bool prezzoARichiesta = false;
			var strPrezzo = dat[6];
			if (strPrezzo == "NC")
			{
				prezzoARichiesta = true;
			}
			else if (strPrezzo.Contains(";"))
			{
				prezzo = decimal.Parse(strPrezzo.Split(';')[0]);
				prezzoBarrato = decimal.Parse(strPrezzo.Split(';')[1]);
			}
			else
			{
				if (strPrezzo.Trim().Length > 0)
				{
					prezzo = decimal.Parse(strPrezzo);
				}
			}
			var artNew = (new StrumentiMusicali.Library.Entity.Articolo()
			{
				ID = (dat[0]),
				Categoria = int.Parse(dat[1]),
				Condizione = cond,
				Marca = dat[3],
				Titolo = dat[4],
				Testo = dat[5].Replace("<br>", Environment.NewLine),
				Prezzo = prezzo,
				PrezzoARichiesta = prezzoARichiesta,
				PrezzoBarrato = prezzoBarrato,
				BoxProposte = int.Parse(dat[9]) == 1 ? true : false
			});
			if (artNew.ID == "")
			{
				progress++;
				artNew.ID = "Luc_" + DateTime.Now.Ticks.ToString() + progress.ToString();
			}
			uof.ArticoliRepository.Add(artNew);
			var foto = dat[7];
			if (foto.Length > 0)
			{
				int ordine = 0;
				foreach (var item in foto.Split(';'))
				{
					var artFoto = new StrumentiMusicali.Library.Entity.FotoArticolo()
					{
						Articolo = artNew,
						UrlFoto = item,
						Ordine = ordine
					};
					ordine++;
					uof.FotoArticoloRepository.Add(artFoto);
				}
			}

			return progress;
		}

		private void DeleteArticolo(object obj)
		{
			try
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare l'articolo selezionato?"))
					return;
				using (var save = new SaveEntityManager())
				{
					using (var immaginiController = new ControllerImmagini())
					{


						var item = save.UnitOfWork.ArticoliRepository.Find(a => a.ID ==
						this.SelectedItem.ID).FirstOrDefault();
						_logger.Info(string.Format("Cancellazione articolo /r/n{0} /r/n{1}", item.Titolo, item.ID));

						if (!immaginiController.CheckFolderImmagini())
							return;
						var folderFoto = ReadSetting().settingSito.CartellaLocaleImmagini;
						var listFile = new List<string>();
						foreach (var itemFoto in save.UnitOfWork.FotoArticoloRepository.Find(a => a.ArticoloID == item.ID))
						{
							immaginiController.RimuoviItemDaRepo(
								folderFoto, listFile, save.UnitOfWork, itemFoto);
						}
						immaginiController.DeleteFile(listFile);

						save.UnitOfWork.ArticoliRepository.Delete(item);
						if (save.SaveEntity(enSaveOperation.OpDelete))
						{
							EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>());
						}
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void SaveArticolo(object arg)
		{
			try
			{
				using (var save = new SaveEntityManager())
				{
					var uof = save.UnitOfWork;
					if (string.IsNullOrEmpty(EditItem.ID)
						|| uof.ArticoliRepository.Find(a => a.ID == EditItem.ID).Count() == 0)
					{
						uof.ArticoliRepository.Add(EditItem);
					}
					else
					{
						uof.ArticoliRepository.Update(EditItem);
					}
					if (
					save.SaveEntity(enSaveOperation.OpSave))
					{

					}
				}
			}
			catch (MessageException ex)
			{
				ExceptionManager.ManageError(ex);
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void AggiungiImmagine(ImageAdd eventArg)
		{
			try
			{
				using (OpenFileDialog res = new OpenFileDialog())
				{
					res.Title = "Seleziona file da importare";
					//Filter
					res.Filter = "File jpg e jpeg|*.jpg;*.jepg|Tutti i file|*.*";

					res.Multiselect = true;
					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						EventAggregator.Instance().Publish<ImageAddFiles>(
							new ImageAddFiles(eventArg.Articolo, res.FileNames.ToList()));
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		public override void RefreshList(UpdateList<Articolo> obj)
		{
			try
			{
				var datoRicerca = TestoRicerca;
				List<ArticoloItem> list = new List<ArticoloItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.ArticoliRepository.Find(a => datoRicerca == "" || a.Titolo.Contains(datoRicerca)
						|| a.Testo.Contains(datoRicerca)
					).Select(a => new ArticoloItem
					{
						ID = a.ID,
						Titolo = a.Titolo,
						Entity = a,
						DataCreazione = a.DataCreazione,
						DataModifica = a.DataUltimaModifica,
						//Pinned = a.Pinned
					}).ToList();
				}

				DataSource = new View.Utility.MySortableBindingList<ArticoloItem>(list);
			}
			catch (Exception ex)
			{
				new Task(new Action(() =>
				{ ExceptionManager.ManageError(ex); })).Wait();

			}
		}
	}
}