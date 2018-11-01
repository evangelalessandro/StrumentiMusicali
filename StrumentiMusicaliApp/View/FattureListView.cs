using NLog;

using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Utility;
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
	public partial class FattureListView : UserControl, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		private ControllerFatturazione _baseController;

		private MenuTab _menuTab = null;

		private RibbonMenuButton ribCercaArticolo;

		private RibbonMenuButton ribDeleteArt;

		private RibbonMenuButton ribDuplicaArt;

		private RibbonMenuButton ribEditArt;

		public FattureListView(ControllerFatturazione baseController)
													: base()
		{
			_baseController = baseController;
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;

			this.Load += (this.Form_Load);
			txtCerca.KeyUp += TxtCerca_KeyUp;
			this.Disposed += FattureListView_Disposed;
			EventAggregator.Instance().Subscribe<FattureListUpdate>(UpdateList);
			_logger.Debug(this.Name + " init");
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

				return (list);
			}
			catch (Exception ex)
			{
				this.BeginInvoke(new Action(() =>
			   { ExceptionManager.ManageError(ex); }));
				return null;
			}
		}

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				AggiungiComandi();
			}
			return _menuTab;
		}

		private void AggiungiComandi()
		{
			var tabArticoli = _menuTab.Add("Principale");
			var panelComandiArticoli = tabArticoli.Add("Comandi");
			var ribCreArt = panelComandiArticoli.Add("Crea", Properties.Resources.Add);
			ribEditArt = panelComandiArticoli.Add(@"Vedi\Modifica", Properties.Resources.Edit);
			ribDeleteArt = panelComandiArticoli.Add("Cancella", Properties.Resources.Delete);
			ribDuplicaArt = panelComandiArticoli.Add("Duplica", Properties.Resources.Duplicate);
			var panelComandiArticoliCerca = tabArticoli.Add("Cerca");
			ribCercaArticolo = panelComandiArticoliCerca.Add("Cerca", Properties.Resources.Find);

			ribCreArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish<NuovaFattura>(new NuovaFattura());
			};
			ribDeleteArt.Click += (a, e) =>
			{
				using (var curs = new CursorManager())
				{
					EventAggregator.Instance().Publish<EliminaFattura>(
						new EliminaFattura(dgvMaster.GetCurrentItemSelected<FatturaItem>()));
				}
			};
			ribEditArt.Click += (a, e) =>
			{
				Edit();
			};

			ribCercaArticolo.Click += (a, e) =>
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
			};
		}

		private void dgvMaster_DoubleClick(object sender, EventArgs e)
		{
			var g = sender as DataGridView;
			if (g != null)
			{
				var p = g.PointToClient(MousePosition);
				var hti = g.HitTest(p.X, p.Y);
				if (hti.Type == DataGridViewHitTestType.ColumnHeader)
				{
					var columnIndex = hti.ColumnIndex;
					//You handled a double click on column header
					//Do what you need
				}
				else if (hti.Type == DataGridViewHitTestType.RowHeader || hti.Type == DataGridViewHitTestType.Cell)
				{
					var rowIndex = hti.RowIndex;
					//You handled a double click on row header
					Edit();
				}
			}
		}

		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			UpdateButtonState();
		}

		private async void Edit()
		{
			var itemSelected = dgvMaster.GetCurrentItemSelected<FatturaItem>();
			EventAggregator.Instance().Publish(new EditFattura(itemSelected));

			await RefreshData();

			await dgvMaster.SelezionaRiga(itemSelected.ID);
		}

		private void FattureListView_Disposed(object sender, EventArgs e)
		{
			var fatt = _baseController.ReadSetting(enAmbienti.FattureList);
			fatt.LastStringaRicerca = txtCerca.Text;
			_baseController.SaveSetting(enAmbienti.FattureList, fatt);
		}

		private async void Form_Load(object sender, EventArgs e)
		{
			var fatt = _baseController.ReadSetting(enAmbienti.FattureList);
			UpdateButtonState();
			await RefreshData();

			await dgvMaster.SelezionaRiga(fatt.LastItemSelected);

			UpdateButtonState();

			_logger.Debug("Form load");
		}

		private async Task RefreshData()
		{
			var data = await Task.Run(() => { return GetDataAsync(); });
			if (data == null)
				return;
			dgvMaster.DataSource = new MySortableBindingList<FatturaItem>(data);

			dgvMaster.Columns["Entity"].Visible = false;
			dgvMaster.AutoResizeColumns();
			dgvMaster.Columns["ID"].DisplayIndex = 0;
			dgvMaster.Columns["Codice"].DisplayIndex = 1;
			dgvMaster.Columns["RagioneSociale"].DisplayIndex = 2;
		}

		private async void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				await RefreshData();
			}
		}

		private void UpdateButtonState()
		{
			if (_menuTab != null)
			{
				_menuTab.Enabled = !(dgvMaster.DataSource == null);

				ribEditArt.Enabled = dgvMaster.SelectedRows.Count > 0;
				ribDeleteArt.Enabled = dgvMaster.SelectedRows.Count > 0;
				ribCercaArticolo.Checked = pnlCerca.Visible;
			}
		}

		private async void UpdateList(FattureListUpdate obj)
		{
			await RefreshData();

			await dgvMaster.SelezionaRiga(_baseController.SelectedItem.ID.ToString());
		}
	}
}