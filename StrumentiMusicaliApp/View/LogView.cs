using NLog;

using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public partial class LogView : BaseGridViewGeneric<LogItem, ControllerLog, EventLog>, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		private ControllerLog _baseController;

		private MenuTab _menuTab = null;

		private RibbonMenuButton ribCercaArticolo;

		private RibbonMenuButton ribDeleteArt;


		public LogView(ControllerLog baseController)
			: base(baseController)
		{
			_baseController = baseController;
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;

			this.Load += (this.control_Load);
			txtCerca.KeyUp += TxtCerca_KeyUp;
			this.Disposed += FattureListView_Disposed;
			_logger.Debug(this.Name + " init");
			this.Load += FattureRigheListView_Load;

		}

		private void FattureRigheListView_Load(object sender, System.EventArgs e)
		{
			_baseController.RefreshList(null);
			dgvRighe.DataSource = _baseController.DataSource;

			dgvRighe.Refresh();
			EventAggregator.Instance().Subscribe<UpdateList<EventLog>>(RefreshList);

			dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			for (int i = 0; i < dgvRighe.ColumnCount; i++)
			{
				dgvRighe.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			}
			dgvRighe.Columns["ID"].DisplayIndex = 0;
			dgvRighe.Columns["DataCreazione"].DisplayIndex = 1;

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
			ribDeleteArt = panelComandiArticoli.Add("Cancella", Properties.Resources.Delete);
			var panelComandiArticoliCerca = tabArticoli.Add("Cerca");
			ribCercaArticolo = panelComandiArticoliCerca.Add("Cerca", Properties.Resources.Find);

			ribDeleteArt.Click += (a, e) =>
			{

				EventAggregator.Instance().Publish<Remove<LogItem, EventLog>>(
					new Remove<LogItem, EventLog>());

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

				}
			}
		}

		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			UpdateButtonState();
		}


		private void FattureListView_Disposed(object sender, EventArgs e)
		{
			var fatt = _baseController.ReadSetting(enAmbienti.LogView);
			fatt.LastStringaRicerca = txtCerca.Text;
			_baseController.SaveSetting(enAmbienti.LogView, fatt);
		}


		private void control_Load(object sender, System.EventArgs e)
		{
			_baseController.RefreshList(null);
			dgvRighe.DataSource = _baseController.DataSource;

			dgvRighe.Refresh();
			EventAggregator.Instance().Subscribe<UpdateList<EventLog>>(RefreshList);

			dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		private void RefreshList(UpdateList<EventLog> obj)
		{
			this.Invalidate();

			dgvRighe.DataSource = _baseController.DataSource;

			dgvRighe.Refresh();
			dgvRighe.Update();
		}
		private async void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				_baseController.TestoRicerca = txtCerca.Text;
				EventAggregator.Instance().Publish<UpdateList<EventLog>>(new UpdateList<EventLog>());

			}
		}

		private void UpdateButtonState()
		{
			if (_menuTab != null)
			{
				_menuTab.Enabled = (dgvMaster.DataSource == null);

				ribDeleteArt.Enabled = dgvMaster.SelectedRows.Count > 0;
				ribCercaArticolo.Checked = pnlCerca.Visible;
			}
		}

	}
}