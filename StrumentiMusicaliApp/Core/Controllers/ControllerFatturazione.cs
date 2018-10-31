using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.Fatture;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerFatturazione : BaseController
	{
		public Fattura SelectedItem { get; set; } = new Fattura();

		public ControllerFatturazione() : base()
		{
			EventAggregator.Instance().Subscribe<ImportaFattureAccess>(ImportaFatture);
			EventAggregator.Instance().Subscribe<ApriAmbiente>(ApriAmbiente);
			EventAggregator.Instance().Subscribe<NuovaFattura>(AddFattura);
			EventAggregator.Instance().Subscribe<EditFattura>(FatturaEdit);
			EventAggregator.Instance().Subscribe<FatturaSave>(Save);

			EventAggregator.Instance().Subscribe<EliminaFattura>(DelFattura);
		}

		private void Save(FatturaSave obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				if (SelectedItem.Pagamento.Length == 0)
				{
					MessageManager.NotificaWarnig("Occorre impostare il pagamento");
					return;
				}

				var uof = saveManager.UnitOfWork;
				SelectedItem.Data = SelectedItem.Data.Date;

				if (SelectedItem.ID > 0)
				{
					uof.FatturaRepository.Update(SelectedItem);
				}
				else
				{
					uof.FatturaRepository.Add(SelectedItem);
				}

				if (saveManager.SaveEntity(enSaveOperation.OpSave))
				{
					EventAggregator.Instance().Publish<FattureListUpdate>(new FattureListUpdate());
				}
			}
		}

		~ControllerFatturazione()
		{
			var dato = this.ReadSetting(Settings.enAmbienti.FattureList);
			if (SelectedItem != null && SelectedItem != null && SelectedItem.ID > 0)
			{
				dato.LastItemSelected = SelectedItem.ID.ToString();
				this.SaveSetting(Settings.enAmbienti.FattureList, dato);
			}
		}

		private void DelFattura(EliminaFattura obj)
		{
			try
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare la fattura selezionata?"))
					return;
				using (var saveEntity = new SaveEntityManager())
				{
					var uof = saveEntity.UnitOfWork;
					int val = int.Parse(obj.ItemSelected.ID);
					var item = uof.FatturaRepository.Find(a => a.ID == val).FirstOrDefault();
					_logger.Info(string.Format("Cancellazione fattura/r/n codice {0} /r/n Numero {1}",
						item.Codice, item.ID));
					uof.FatturaRepository.Delete(item);

					if (saveEntity.SaveEntity(enSaveOperation.OpDelete))
					{
						EventAggregator.Instance().Publish<FattureListUpdate>(
							new FattureListUpdate());
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void FatturaEdit(EditFattura obj)
		{
			SelectedItem = ((FatturaItem)obj.ItemSelected).Entity;
			ShowDettaglio();
		}

		private void ShowDettaglio()
		{
			using (var view = new DettaglioFatturaView(this))
			{
				ShowView(view, Settings.enAmbienti.Fattura);
			}
		}

		private void AddFattura(NuovaFattura obj)
		{
			SelectedItem = new Fattura();
			ShowDettaglio();
		}

		private void ApriAmbiente(ApriAmbiente obj)
		{
			if (obj.TipoEnviroment == enTipoEnviroment.Fatturazione)
			{
				using (var view = new FattureListView(this))
				{
					ShowView(view, Settings.enAmbienti.FattureList);
				}
			}
		}

		private void ImportaFatture(ImportaFattureAccess obj)
		{
			try
			{
				using (OpenFileDialog res = new OpenFileDialog())
				{
					res.Title = "Seleziona file da importare";
					//Filter
					res.Filter = "File access|*.mdb|Tutti i file|*.*";

					res.Multiselect = false;
					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						using (var dat = new CursorManager())
						{
							using (var importa = new ImportFatture())
							{
								importa.ImportAccessDB(res.FileName);
							}
						}
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