﻿using NLog;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public partial class FattureListView : Form
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		private ControllerFatturazione _baseController;

		public FattureListView(ControllerFatturazione baseController)
		{
			_baseController = baseController;
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;
			ribCerca.Click += RibCerca_Click;
			ribDelete.Click += (s, e) =>
			{
				using (var curs = new CursorManager())
				{
					EventAggregator.Instance().Publish<EliminaFattura>(new EliminaFattura(GetCurrentItemSelected()));
				}
			};

			ribAddArt.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish<NuovaFattura>(new NuovaFattura());
			};
			ribEditArt.Click += (s, e) =>
			{
				Edit();
			};

			this.Load += (this.Form_Load);
			txtCerca.KeyUp += TxtCerca_KeyUp;
			EventAggregator.Instance().Subscribe<FattureListUpdate>(UpdateList);
			this.Disposed += FmrMain_Disposed;
			_logger.Debug(this.Name + " init");
		}

		private void FmrMain_Disposed(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting(Settings.enAmbienti.FattureList);

			dato.FormMainWindowState = this.WindowState;
			dato.SizeFormMain = this.Size;
			dato.LastStringaRicerca = txtCerca.Text;
			_baseController.SaveSetting(Settings.enAmbienti.FattureList, dato);
		}

		private async void UpdateList(FattureListUpdate obj)
		{
			await RefreshData();
		}

		private async void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				
				await RefreshData();
			}
		}

		private void RibCerca_Click(object sender, EventArgs e)
		{
			bool visibleCerca = pnlCerca.Visible;
			pnlCerca.Visible = !visibleCerca;
			splitter1.Visible = !visibleCerca;
			if (!visibleCerca)
			{
				pnlArticoli.Dock = DockStyle.Fill;
			}
			else
			{
				pnlCerca.Dock = DockStyle.Top;
			}
			UpdateButtonState();
		}

		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			UpdateButtonState();

		}

		private FatturaItem GetCurrentItemSelected()
		{
			FatturaItem item = null;
			if (dgvMaster.SelectedRows.Count > 0)
			{
				item = (FatturaItem)dgvMaster.SelectedRows[0].DataBoundItem;
			}
			return item;
		}

		private void UpdateButtonState()
		{
			ribbon1.Enabled = !(dgvMaster.DataSource == null);

			ribEditArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribDelete.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribCerca.Checked = pnlCerca.Visible;
		}

		private async void Form_Load(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting(Settings.enAmbienti.FattureList);


			txtCerca.Text = dato.LastStringaRicerca;

			try
			{
				this.WindowState = dato.FormMainWindowState;
				this.Size = dato.SizeFormMain;
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
			UpdateButtonState();
			await RefreshData();

			await SelezionaRiga(dato.LastItemSelected);

			UpdateButtonState();

			_logger.Debug("Form load");
		}

		public List<FatturaItem> GetDataAsync()
		{
			try
			{
				var datoRicerca = txtCerca.Text;
				List<FatturaItem> list = new List<FatturaItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.FatturaRepository.Find(a => datoRicerca == ""
					   || a.RagioneSociale.Contains(datoRicerca)
						|| a.PIVA.Contains(datoRicerca)
						|| a.Codice.Equals(datoRicerca, StringComparison.InvariantCultureIgnoreCase)

					).Select(a => new FatturaItem
					{
						ID = a.ID.ToString(),
						Data = a.Data,
						FatturaCS = a,
						PIVA = a.PIVA,
						RagioneSociale = a.RagioneSociale
					}).Take(100).OrderByDescending(a => a.Data).ToList();
				}

				return list;
			}
			catch (Exception ex)
			{
				this.BeginInvoke(new Action(() =>
			   { ExceptionManager.ManageError(ex); }));
				return null;
			}
		}

		private async Task RefreshData()
		{
			var data = await Task.Run(() => { return GetDataAsync(); });
			if (data == null)
				return;
			dgvMaster.DataSource = data;

		}



		 

		 
		private async Task SelezionaRiga(string idItem)
		{
			for (int i = 0; i < dgvMaster.RowCount; i++)
			{
				if (((FatturaItem)(dgvMaster.Rows[i].DataBoundItem)).ID == idItem)
				{
					dgvMaster.Rows[i].Selected = true;
					dgvMaster.CurrentCell = dgvMaster.Rows[i].Cells[1];
					break;
				}
			}
		}

		private void dgvMaster_DoubleClick(object sender, EventArgs e)
		{
			Edit();
		}

		private void Edit()
		{
			var itemSelected = GetCurrentItemSelected();
			EventAggregator.Instance().Publish<EditFattura>(new EditFattura(itemSelected));

			RefreshData();
			SelezionaRiga(itemSelected.ID);
		}
	}
}