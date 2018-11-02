using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.Fatture;
using StrumentiMusicali.App.Core.Controllers.Stampa;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	[AddINotifyPropertyChangedInterface]
	public class ControllerFatturazione : BaseControllerGeneric<Fattura,FatturaItem>
	{ 
		
		public ControllerFatturazione() : 
			base(enAmbienti.FattureList,enAmbienti.Fattura)
		{
			EventAggregator.Instance().Subscribe<ImportaFattureAccess>(ImportaFatture);
			EventAggregator.Instance().Subscribe<ApriAmbiente>(ApriAmbiente);
			EventAggregator.Instance().Subscribe<Add<FatturaItem, Fattura>>(AddFattura);
			EventAggregator.Instance().Subscribe<Edit<FatturaItem,Fattura>>(FatturaEdit);
			EventAggregator.Instance().Subscribe<Save<FatturaItem, Fattura>>(Save);

			EventAggregator.Instance().Subscribe<Remove<FatturaItem, Fattura>>(DelFattura);
			///comando di stampa
			AggiungiComandi();
		}

		private void Save(Save<FatturaItem, Fattura> obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				if (string.IsNullOrEmpty(EditItem.Pagamento))
				{
					MessageManager.NotificaWarnig("Occorre impostare il pagamento");
					return;
				}

				var uof = saveManager.UnitOfWork;
				EditItem.Data = EditItem.Data.Date;

				if (EditItem.ID > 0)
				{
					uof.FatturaRepository.Update(EditItem);
				}
				else
				{
					uof.FatturaRepository.Add(EditItem);
				}

				if (saveManager.SaveEntity(enSaveOperation.OpSave))
				{
					EventAggregator.Instance().Publish<UpdateList<Fattura>>(
							new UpdateList<Fattura>());
				}
			}
		}
		
		~ControllerFatturazione()
		{
			
		}

		private void DelFattura(Remove<FatturaItem, Fattura> obj)
		{
			try
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare la fattura selezionata?"))
					return;
				using (var saveEntity = new SaveEntityManager())
				{
					var uof = saveEntity.UnitOfWork;
					int val = ((Fattura) SelectedItem).ID;
					var item = uof.FatturaRepository.Find(a => a.ID == val).FirstOrDefault();
					_logger.Info(string.Format("Cancellazione fattura/r/n codice {0} /r/n Numero {1}",
						item.Codice, item.ID));

					foreach (var itemRiga in uof.FattureRigheRepository.Find(a => a.FatturaID == val))
					{
						uof.FattureRigheRepository.Delete(itemRiga);
					}

					uof.FatturaRepository.Delete(item);

					if (saveEntity.SaveEntity(enSaveOperation.OpDelete))
					{
						EventAggregator.Instance().Publish<UpdateList<Fattura>>(
							new UpdateList<Fattura>());
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void FatturaEdit(Edit<FatturaItem, Fattura> edit)
		{ 
			EditItem = (Fattura) SelectedItem;
			ShowDettaglio();
		}

		private void ShowDettaglio()
		{
			using (var view = new DettaglioFatturaView(this))
			{
				ShowView(view, Settings.enAmbienti.Fattura);
			}
		}

		private void AddFattura(Add<FatturaItem, Fattura> obj)
		{
			EditItem = new Fattura();
			ShowDettaglio();
		}

		private void ApriAmbiente(ApriAmbiente obj)
		{
			if (obj.TipoEnviroment == enAmbienti.FattureList)
			{
				using (var view = new FattureListView(this, enAmbienti.FattureList, enAmbienti.Fattura))
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

		internal void StampaFattura(Fattura fattura)
		{
			_logger.Info("Avvio stampa");
			try
			{
				using (var cursorManger = new CursorManager())
				{
					using (var stampa = new StampaFattura())
					{
						stampa.Stampa(
							ReadSetting().DatiIntestazione,
							fattura);

						_logger.Info("Stampa completata correttamente.");
					}
				}

			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
		private void AggiungiComandi()
		{
			var pnlStampa = GetMenu().Tabs[0].Add("Stampa");
			var ribStampa = pnlStampa.Add("Avvia stampa", Properties.Resources.Print_48,true);
			ribStampa.Click += (a, e) =>
			{
				StampaFattura((Fattura)SelectedItem);
			};
		}

		public override void RefreshList(UpdateList<Fattura> obj)
		{
			 
			try
			{
				var datoRicerca = TestoRicerca;
				var list = new List<FatturaItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.FatturaRepository.Find(a => datoRicerca == ""
					   || a.RagioneSociale.Contains(datoRicerca)
						|| a.PIVA.Contains(datoRicerca)
						|| a.Codice.Contains(datoRicerca)

					).Select(a => new FatturaItem
					{
						ID = a.ID.ToString(),
						Data = a.Data,
						Entity = a,
						PIVA = a.PIVA,
						Codice = a.Codice,
						RagioneSociale = a.RagioneSociale
					}).OrderByDescending(a => a.ID).Take(100).ToList();
				}

				DataSource=new View.Utility.MySortableBindingList<FatturaItem>( list);
			}
			catch (Exception ex)
			{
				new Task(new Action(() =>
				{ ExceptionManager.ManageError(ex); })).Wait();
				
			}
		}
	}
}