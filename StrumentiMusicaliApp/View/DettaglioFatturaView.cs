using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public partial class DettaglioFatturaView : UserControl, IMenu
	{
		private ControllerFatturazione _controllerFatturazione;

		private ControllerRigheFatture _controllerRighe;

		public DettaglioFatturaView(ControllerFatturazione controllerFatturazione)
			: base()
		{
			_controllerFatturazione = controllerFatturazione;
			InitializeComponent();
			
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
			_controllerRighe = new ControllerRigheFatture(_controllerFatturazione);
			var controlFattRigheList = new FattureRigheListView(_controllerRighe);
			controlFattRigheList.Height = tabPage2.Size.Height;
			controlFattRigheList.Dock = DockStyle.Top;
			tabPage2.Controls.Add(controlFattRigheList);

			FillCombo();

			UpdateButtonState();

			UtilityView.SetDataBind(this, _controllerFatturazione.SelectedItem);

			txtRagioneSociale.TextChanged += TxtRagioneSociale_TextChanged;


			using (var ord=new Utility.OrdinaTab())
			{
				ord.OrderTab(pnl1Alto);
				ord.OrderTab(pnl2Testo);
				ord.OrderTab(pnl3Basso);

			}

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
			_controllerRighe.UpdateDataSource();
		}

		

		

		private void UpdateButtonState()
		{
			if (_menuTab!=null)
			{ 
				_menuTab.Tabs[0].Pannelli[0].Enabled = tabControl1.SelectedTab == tabPage2;
				
			}
		}
		MenuTab _menuTab = null;
		public MenuTab GetMenu()
		{
			if (_menuTab==null)
			{
				_menuTab = new MenuTab();
				
				var tab =_menuTab.Add("Principale");
				var ribPannel =tab.Add("Principale");
				var ribSave = ribPannel.Add("Save", Properties.Resources.Save);
				ribSave.Click += (a, e) =>
				{
					this.Validate();
					EventAggregator.Instance().Publish<FatturaSave>(new FatturaSave());
					UpdateButtonState();
				};
			}
			return _menuTab;
			 
		}
	}
}