using StrumentiMusicaliApp.Core;
using StrumentiMusicaliApp.Core.Controllers;
using StrumentiMusicaliApp.Core.Events.Articoli;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace StrumentiMusicaliApp
{
	public partial class fmrMain : Form
	{
		private BaseController _baseController;
		public fmrMain(BaseController baseController)
		{
			_baseController = baseController;
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;
			ribCerca.Click += RibCerca_Click;
			ribDelete.Click += ribDelete_Click;
			ribImportArticoli.Click += RibImportArticoli_Click;
			txtCerca.KeyUp += TxtCerca_KeyUp;
			EventAggregator.Instance().Subscribe<ArticoliToUpdate>(UpdateList);
			this.Disposed += FmrMain_Disposed;
		}

		private void FmrMain_Disposed(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting();
			dato.FormMainWindowState = this.WindowState;
			dato.SizeFormMain = this.Size;
			_baseController.SaveSetting(dato);
		}

		private void RibImportArticoli_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImportArticoli>(new ImportArticoli());

		}

		private void ribDelete_Click(object sender, EventArgs e)
		{
			using (var curs = new CursorHandler())
			{
				EventAggregator.Instance().Publish<ArticoloDelete>(new ArticoloDelete(GetCurrentItemSelected()));
			}
		}

		private void UpdateList(ArticoliToUpdate obj)
		{
			RefreshData();
		}

		private void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				var dato = _baseController.ReadSetting();
				dato.LastStringaRicerca = txtCerca.Text;
				_baseController.SaveSetting(dato);
				RefreshData();
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
			ribEditArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribArtDuplicate.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribDelete.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribCerca.Checked = pnlCerca.Visible;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting();
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
			Init(dato.LastArticoloSelected);
		}
		private void Init(string idKey)
		{
			RefreshData();
			UpdateButtonState();

			SelezionaRiga(idKey);

		}
		public List<ArticoloItem> GetDataAsync()
		{
			var dt = new DataTable();
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
		private async void RefreshData()
		{
			using (var curs = new CursorHandler())
			{
				var datoRicerca = txtCerca.Text;
				using (var uof = new UnitOfWork())
				{
					//var list = uof.ArticoliRepository.Find(a => datoRicerca == "" || a.Titolo.Contains(datoRicerca)
					//	|| a.Testo.Contains(datoRicerca)
					//).Select(a => new ArticoloItem
					//{
					//	ID = a.ID,
					//	Titolo = a.Titolo,
					//	ArticoloCS = a
					//	,
					//	DataCreazione = a.DataCreazione,
					//	DataModifica = a.DataUltimaModifica
					//	,
					//	Pinned = a.Pinned
					//}).ToList();
					//dgvMaster.DataSource = list;

					var data = GetDataAsync();
					dgvMaster.DataSource = data;
					dgvMaster.Columns[0].Visible = false;
					dgvMaster.Columns["Pinned"].Visible = false;
					dgvMaster.Columns["ArticoloCS"].Visible = false;
				}
			}
		}
		private readonly SynchronizationContext synchronizationContext;
		private DateTime previousTime = DateTime.Now;

		private void ribAddArt_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish(new ArticoloAdd());
		}


		private void ribArtDuplicate_Click(object sender, EventArgs e)
		{
			using (var curs = new CursorHandler())
			{
				EventAggregator.Instance().Publish(new ArticoloDuplicate());
			}
		}

		private void ribEditArt_Click(object sender, EventArgs e)
		{
			EditArticolo();
		}

		private void EditArticolo()
		{
			var itemSelected = (ArticoloItem)dgvMaster.SelectedRows[0].DataBoundItem;
			using (var frm = new Forms.frmArticolo(itemSelected))
			{
				frm.ShowDialog();
			}

			RefreshData();
			SelezionaRiga(itemSelected.ID);
		}

		private void SelezionaRiga(string idArticolo)
		{
			for (int i = 0; i < dgvMaster.RowCount; i++)
			{
				if (((ArticoloItem)(dgvMaster.Rows[i].DataBoundItem)).ID == idArticolo)
				{
					dgvMaster.Rows[i].Selected = true;
					dgvMaster.CurrentCell = dgvMaster.Rows[i].Cells["Titolo"];
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

