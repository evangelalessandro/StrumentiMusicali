using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.View.Base;
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
	public partial class DettaglioFatturaView : BaseDataControl
	{
		private ControllerFatturazione _controllerFatturazione;

		public DettaglioFatturaView(ControllerFatturazione controllerFatturazione)
			: base()
		{
			_controllerFatturazione = controllerFatturazione;
			InitializeComponent();

			ribSave.Click += (a, b) =>
			{
				this.Validate();
				EventAggregator.Instance().Publish<FatturaSave>(new FatturaSave());
				UpdateButtonState();
			};
			ribRemove.Click+= (a, b) =>
			{
				this.Validate();
				EventAggregator.Instance().Publish<FatturaSave>(new FatturaSave());
				UpdateButtonState();
			};
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FillCombo()
		{
			using (var uof = new UnitOfWork())
			{
				var listPagamenti = new List<string>();
				listPagamenti.Add(("Seleziona pagamento"));
				listPagamenti.Add(("Bonifico Bancario"));
				listPagamenti.Add(("Rimessa Diretta"));

				cboPagamento.DataSource = listPagamenti.Select(a =>
						new
						{
							ID = a,
							Descrizione = a
						}
						).ToList();
				cboPagamento.DisplayMember = "Descrizione";
				cboPagamento.ValueMember = "ID";

				txtAspettoBeni.Values = uof.FatturaRepository.Find(a => true).Select(a => a.AspettoEsterno).Distinct().ToList().ToArray();
				txtCausaleTrasporto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.CausaleTrasporto).Distinct().ToList().ToArray();
				txtPorto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Porto).Distinct().ToList().ToArray();
				txtTrasportoACura.Values = uof.FatturaRepository.Find(a => true).Select(a => a.TrasportoACura).Distinct().ToList().ToArray();
				txtVettore.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Vettore).Distinct().ToList().ToArray();
				txtNote1.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note1).Distinct().ToList().ToArray();
				txtNote2.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note2).Distinct().ToList().ToArray();

				txtRagioneSociale.Values =
					uof.ClientiRepository.Find(a => true).Select(a => a.RagioneSociale).Distinct().ToList().ToArray();

			}
		}

		private void TxtRagioneSociale_TextChanged(object sender, EventArgs e)
		{
			using (var uof = new UnitOfWork())
			{
				var finded = uof.ClientiRepository.Find(a => a.RagioneSociale == txtRagioneSociale.Text).Select(a => a.PIVA).FirstOrDefault();
				if (finded != null && finded.Length > 0)
				{
					txtPIVA.Text = finded;
				}
			}
		}

		private void frm_Load(object sender, EventArgs e)
		{
			FillCombo();

			UpdateButtonState();

			SetDataBind(this, _controllerFatturazione.SelectedItem);

			txtRagioneSociale.TextChanged += TxtRagioneSociale_TextChanged;



			OrderTab(pnl1Alto);
			OrderTab(pnl2Testo);
			OrderTab(pnl3Basso);

		}



		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedTab == tabPage2)
			{
				RefreshRighe();
			}
			UpdateButtonState();
		}

		private void RefreshRighe()
		{
			RefreshData();
		}

		public List<FatturaRigaItem> GetDataAsync()
		{
			try
			{ 
				List<FatturaRigaItem> list = new List<FatturaRigaItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.FattureRigheRepository.Find(a => a.FatturaID==_controllerFatturazione.SelectedItem.ID

					).Select(a => new FatturaRigaItem
					{
						ID = a.ID.ToString(),
						CodiceArt=a.CodiceArticoloOld,
						Descrizione=a.Descrizione,
						FatturaRigaCS=a,
						Importo=a.Qta*a.PrezzoUnitario,
						PrezzoUnitario=a.PrezzoUnitario,
						Qta=a.Qta,
						Iva=a.IvaApplicata
					}).OrderBy(a => a.FatturaRigaCS.OrdineVisualizzazione).ThenBy(a=>a.ID).ToList();
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
			dgvRighe.DataSource = data;

			var provider = new System.Globalization.CultureInfo("it-IT");
			//var provider = new System.Globalization.CultureInfo("en");

			dgvRighe.Columns["PrezzoUnitario"].DefaultCellStyle.FormatProvider = provider;
			dgvRighe.Columns["PrezzoUnitario"].DefaultCellStyle.Format = "C2";
			dgvRighe.Columns["PrezzoUnitario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgvRighe.Columns["Importo"].DefaultCellStyle.FormatProvider = provider;
			dgvRighe.Columns["Importo"].DefaultCellStyle.Format = "C2";
			dgvRighe.Columns["Importo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgvRighe.Columns["FatturaRigaCS"].Visible = false;
			dgvRighe.Columns["ID"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.Columns["CodiceArt"].DisplayIndex = 0;
			dgvRighe.Columns["Descrizione"].DisplayIndex = 1;
			dgvRighe.Columns["Qta"].DisplayIndex = 2;
			dgvRighe.Columns["PrezzoUnitario"].DisplayIndex = 3;
			dgvRighe.Columns["Importo"].DisplayIndex = 4;
			dgvRighe.Columns["Iva"].DisplayIndex = 5;
		}

		private void UpdateButtonState()
		{
			ribPanelRighe.Enabled = tabControl1.SelectedTab == tabPage2;
			ribRemove.Enabled = true;
		}
	}
}