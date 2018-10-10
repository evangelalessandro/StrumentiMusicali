using NLog;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Repo;
using StrumentiMusicaliApp.Core;
using StrumentiMusicaliApp.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			UpdateButtonState();
			txtCerca.KeyUp += TxtCerca_KeyUp;
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
			splitter1.Visible= !visibleCerca;
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

		private void UpdateButtonState()
		{
			ribEditArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribCerca.Checked = pnlCerca.Visible;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			RefreshData();

		}

		private void RefreshData()
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

		private void ribAddArt_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish(new ArticoloAdd());
		}


		private void ribArtDuplicate_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish(new ArticoloDuplicate());

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

