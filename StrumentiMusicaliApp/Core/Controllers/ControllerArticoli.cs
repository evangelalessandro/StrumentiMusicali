using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Image;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerArticoli : BaseController , IDisposable
	{

		public ControllerArticoli()
			: base()
		{
			EventAggregator.Instance().Subscribe<ArticoloSelected>(ArticoloSelectedChange);

			EventAggregator.Instance().Subscribe<ArticoloAdd>(AggiungiArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDelete>(DeleteArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDuplicate>(DuplicaArticolo);

			EventAggregator.Instance().Subscribe<ImageAdd>(AggiungiImmagine);
			EventAggregator.Instance().Subscribe<ImportArticoli>(ImportaCsvArticoli);
			EventAggregator.Instance().Subscribe<ArticoloSave>(SaveArticolo);

			
		}
		~ControllerArticoli()
		{
			var dato = this.ReadSetting(Settings.enAmbienti.Articoli);
			if (_ArticoloSelected!= null && _ArticoloSelected.ItemSelected != null)
			{
				dato.LastItemSelected = _ArticoloSelected.ItemSelected.ID;
				this.SaveSetting(Settings.enAmbienti.Articoli,dato);
			}
		}
		private ArticoloSelected _ArticoloSelected;

		private void ArticoloSelectedChange(ArticoloSelected obj)
		{
			_ArticoloSelected = obj;

			
		}

		private void AggiungiArticolo(ArticoloAdd articoloAdd)
		{
			_logger.Info("Apertura ambiente AggiungiArticolo");

			using (var frm = new Forms.DettaglioArticoloView())
			{
				frm.ShowDialog();
			}
		}

		private void DuplicaArticolo(ArticoloDuplicate obj)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					var itemCurrent = ((ArticoloItem)(_ArticoloSelected.ItemSelected)).ArticoloCS;
					uof.ArticoliRepository.Add(new StrumentiMusicali.Library.Entity.Articolo()
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
					uof.Commit();
				}
				MessageManager.NotificaInfo("Duplicazione avvenuta con successo");
				_logger.Info("Duplica articolo");
				EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
		private void ImportaCsvArticoli(ImportArticoli obj)
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
						EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());
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

		private void DeleteArticolo(ArticoloDelete obj)
		{
			try
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare l'articolo selezionato?"))
					return;
				using (var uof = new UnitOfWork())
				{
					var item = uof.ArticoliRepository.Find(a => a.ID == obj.ItemSelected.ID).FirstOrDefault();
					_logger.Info(string.Format("Cancellazione articolo /r/n{0} /r/n{1}", item.Titolo, item.ID));
					uof.ArticoliRepository.Delete(item);
					uof.Commit();
				}
				MessageManager.NotificaInfo("Cancellazione avvenuta correttamente!");
				EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void SaveArticolo(ArticoloSave arg)
		{
			try
			{
				using (var curs = new CursorManager())
				{
					using (var uof = new UnitOfWork())
					{
						if (string.IsNullOrEmpty(arg.Articolo.ID)
							|| uof.ArticoliRepository.Find(a => a.ID == arg.Articolo.ID).Count() == 0)
						{
							uof.ArticoliRepository.Add(arg.Articolo);
						}
						else
						{
							uof.ArticoliRepository.Update(arg.Articolo);
						}
						uof.Commit();
					}
				}
				MessageManager.NotificaInfo("Salvataggio avvenuto con successo");
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

	}
}
