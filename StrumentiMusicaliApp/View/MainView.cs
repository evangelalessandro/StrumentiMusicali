using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App
{
	public partial class MainView : Form
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		private BaseController _baseController;

		public MainView(BaseController baseController)
		{
			_baseController = baseController;
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;
			ribCerca.Click += RibCerca_Click;
			ribDelete.Click += (s, e) =>
			{
				using (var curs = new CursorManager())
				{
					EventAggregator.Instance().Publish<ArticoloDelete>(new ArticoloDelete(GetCurrentItemSelected()));
				}
			};
			ribImportArticoli.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish<ImportArticoli>(new ImportArticoli());
			};
			ribCaricaMagazzino.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriGestioneMagazzino());
			};
			ribCaricaMagazzino.LargeImage = StrumentiMusicali.App.Properties.Resources.Magazzino;

			ribImportFattureAccess.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportaFattureAccess());
			};
			ribBtnApriFatturazione.LargeImage = StrumentiMusicali.App.Properties.Resources.Apri_Fatturazione;
			ribBtnApriFatturazione.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriFatturazione());
			};

			txtCerca.KeyUp += TxtCerca_KeyUp;
			EventAggregator.Instance().Subscribe<ArticoliToUpdate>(UpdateList);
			this.Disposed += FmrMain_Disposed;
			_logger.Debug("Form main init");
		}

		private void FmrMain_Disposed(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting(Settings.enAmbienti.Main);
			dato.FormMainWindowState = this.WindowState;
			dato.SizeFormMain = this.Size;
			_baseController.SaveSetting(Settings.enAmbienti.Main, dato);
		}

		private async void UpdateList(ArticoliToUpdate obj)
		{
			await RefreshData();
		}

		private async void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				var dato = _baseController.ReadSetting(Settings.enAmbienti.Main);
				dato.LastStringaRicerca = txtCerca.Text;
				_baseController.SaveSetting(Settings.enAmbienti.Main,dato);
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

			EventAggregator.Instance().Publish(new ArticoloSelected(GetCurrentItemSelected()));
		}

		private ArticoloItem GetCurrentItemSelected()
		{
			ArticoloItem item = null;
			if (dgvMaster.SelectedRows.Count > 0)
			{
				item = (ArticoloItem)dgvMaster.SelectedRows[0].DataBoundItem;
			}
			return item;
		}

		private void UpdateButtonState()
		{
			ribbon1.Enabled = !(dgvMaster.DataSource == null);

			ribEditArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribArtDuplicate.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribDelete.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribCerca.Checked = pnlCerca.Visible;
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting(Settings.enAmbienti.Main);
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

		public List<ArticoloItem> GetDataAsync()
		{
			try
			{
				var datoRicerca = txtCerca.Text;
				List<ArticoloItem> list = new List<ArticoloItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.ArticoliRepository.Find(a => datoRicerca == "" || a.Titolo.Contains(datoRicerca)
						|| a.Testo.Contains(datoRicerca)
					).Select(a => new ArticoloItem
					{
						ID = a.ID,
						Titolo = a.Titolo,
						ArticoloCS = a
						,
						DataCreazione = a.DataCreazione,
						DataModifica = a.DataUltimaModifica
						,
						Pinned = a.Pinned
					}).ToList();
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
			dgvMaster.Columns["Pinned"].Visible = false;
			dgvMaster.Columns["ArticoloCS"].Visible = false;
		}

		private void ribAddArt_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish(new ArticoloAdd());
		}

		private void ribArtDuplicate_Click(object sender, EventArgs e)
		{
			using (var curs = new CursorManager())
			{
				EventAggregator.Instance().Publish(new ArticoloDuplicate());
			}
		}

		private void ribEditArt_Click(object sender, EventArgs e)
		{
			EditArticolo();
		}

		private async void EditArticolo()
		{
			var itemSelected = (ArticoloItem)dgvMaster.SelectedRows[0].DataBoundItem;
			using (var frm = new Forms.DettaglioArticoloView(itemSelected))
			{
				frm.ShowDialog();
			}

			await RefreshData();
			await SelezionaRiga(itemSelected.ID);
		}

#pragma warning disable CS1998 // In questo metodo asincrono non sono presenti operatori 'await', pertanto verrà eseguito in modo sincrono. Provare a usare l'operatore 'await' per attendere chiamate ad API non di blocco oppure 'await Task.Run(...)' per effettuare elaborazioni basate sulla CPU in un thread in background.

		private async Task SelezionaRiga(string idArticolo)
#pragma warning restore CS1998 // In questo metodo asincrono non sono presenti operatori 'await', pertanto verrà eseguito in modo sincrono. Provare a usare l'operatore 'await' per attendere chiamate ad API non di blocco oppure 'await Task.Run(...)' per effettuare elaborazioni basate sulla CPU in un thread in background.
		{
			for (int i = 0; i < dgvMaster.RowCount; i++)
			{
				if (((ArticoloItem)(dgvMaster.Rows[i].DataBoundItem)).ID == idArticolo)
				{
					dgvMaster.Rows[i].Selected = true;
					dgvMaster.CurrentCell = dgvMaster.Rows[i].Cells[1];
					break;
				}
			}
		}

		private void dgvMaster_DoubleClick(object sender, EventArgs e)
		{
			EditArticolo();
		}
	}
}