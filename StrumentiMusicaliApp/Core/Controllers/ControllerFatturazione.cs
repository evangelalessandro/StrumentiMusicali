using StrumentiMusicali.App.Core.Controllers.Fatture;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
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
			EventAggregator.Instance().Subscribe<ApriFatturazione>(ApriAmbiente);
			EventAggregator.Instance().Subscribe<NuovaFattura>(AddFattura);
			EventAggregator.Instance().Subscribe<EditFattura>(FatturaEdit);
			EventAggregator.Instance().Subscribe<EliminaFattura>(DelFattura);
		}
		~ControllerFatturazione()
		{
			var dato = this.ReadSetting(Settings.enAmbienti.FattureList);
			if (SelectedItem != null && SelectedItem!= null && SelectedItem.ID>0)
			{
				dato.LastItemSelected = SelectedItem.ID.ToString();
				this.SaveSetting(Settings.enAmbienti.FattureList, dato);
			}
		}
		private void DelFattura(EliminaFattura obj)
		{
			
		}

		private void FatturaEdit( EditFattura obj)
		{
			SelectedItem = ((FatturaItem)obj.ItemSelected).FatturaCS;
			using (var view = new DettaglioFatturaView(this))
			{
				view.ShowDialog();
			}
		}

		private void AddFattura(NuovaFattura obj)
		{
			SelectedItem = new Fattura();
			using (var view=new DettaglioFatturaView(this))
			{

				view.ShowDialog();
			}
		}

		private void ApriAmbiente(ApriFatturazione obj)
		{
			
			using (var view=new FattureListView(this))
			{
				view.ShowDialog();
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
