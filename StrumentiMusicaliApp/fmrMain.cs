using StrumentiMusicaliApp.Core;
using StrumentiMusicaliApp.Core.Events;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Repo;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicaliApp
{
	public partial class fmrMain : Form
	{

		public fmrMain()
		{
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;
			ribCerca.Click += RibCerca_Click;
			ribDelete.Click += ribDelete_Click;
			ribImportArticoli.Click += RibImportArticoli_Click;
			txtCerca.KeyUp += TxtCerca_KeyUp;
			EventAggregator.Instance().Subscribe<ArticoliToUpdate>(UpdateList);

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
				RefreshData();
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
			RefreshData();
			UpdateButtonState();
		}

		private void RefreshData()
		{
			using (var curs = new CursorHandler())
			{
				var datoRicerca = txtCerca.Text;
				using (var uof = new UnitOfWork())
				{
					var list = uof.ArticoliRepository.Find(a => datoRicerca == "" || a.Titolo.Contains(datoRicerca)
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
					dgvMaster.DataSource = list;

					dgvMaster.Columns[0].Visible = false;
					dgvMaster.Columns["Pinned"].Visible = false;
					dgvMaster.Columns["ArticoloCS"].Visible = false;
				}
			}
		}

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
			using (var frm = new Forms.frmArticolo((ArticoloItem)dgvMaster.SelectedRows[0].DataBoundItem))
			{
				frm.ShowDialog();
			}

			var indice = dgvMaster.SelectedRows[0].Index;
			RefreshData();
			dgvMaster.Rows[indice].Selected = true;



		}

	}
}

