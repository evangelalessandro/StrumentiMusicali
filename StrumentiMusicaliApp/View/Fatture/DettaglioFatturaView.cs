﻿using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public partial class DettaglioFatturaView : UserControl, IMenu, ICloseSave
	{
		private ControllerFatturazione _controllerFatturazione;

		private ControllerRigheFatture _controllerRighe;
		private Subscription<RebindItemUpdated<Fattura>> _bindSub;
		public DettaglioFatturaView(ControllerFatturazione controllerFatturazione)
			: base()
		{
			_controllerFatturazione = controllerFatturazione;

			InitializeComponent();
			
			UpdateViewByTipoDocumento();
			_bindSub = EventAggregator.Instance().Subscribe<RebindItemUpdated<Fattura>>((a) => { RebindEditItem(); });
		}
		
		private void UpdateViewByTipoDocumento()
		{
			pnl3Basso.Visible = (_controllerFatturazione.EditItem.TipoDocumento != EnTipoDocumento.DDT);
			lblPagamento.Visible = (_controllerFatturazione.EditItem.TipoDocumento != EnTipoDocumento.DDT);
			cboPagamento.Visible = (_controllerFatturazione.EditItem.TipoDocumento != EnTipoDocumento.DDT);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (_controllerRighe != null)
			{
				_controllerRighe.Dispose();
			}
			EventAggregator.Instance().UnSbscribe(_bindSub);
			_controllerRighe = null;
			foreach (Control item in tabPage2.Controls)
			{
				item.Dispose();
				GC.SuppressFinalize(item);
			}

			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FillCombo()
		{
			FillPagamenti();

			FillTipoDocumenti();

			using (var uof = new UnitOfWork())
			{
				txtAspettoBeni.Values = uof.FatturaRepository.Find(a => true).Select(a => a.AspettoEsterno).Distinct().ToList().ToArray();
				txtCausaleTrasporto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.CausaleTrasporto).Distinct().ToList().ToArray();
				txtPorto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Porto).Distinct().ToList().ToArray();
				txtTrasportoACura.Values = uof.FatturaRepository.Find(a => true).Select(a => a.TrasportoACura).Distinct().ToList().ToArray();
				txtVettore.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Vettore).Distinct().ToList().ToArray();
				txtNote1.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note1).Distinct().ToList().ToArray();
				txtNote2.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note2).Distinct().ToList().ToArray();

				FillCliente(uof);
			}
		}

		private void FillCliente(UnitOfWork uof)
		{
			cboClienteID.Properties.DataSource = uof.ClientiRepository.Find(a => true).Select(a => new { a.ID, RagioneSociale = a.RagioneSociale + " " + a.PIVA }).Distinct().ToList().ToArray();
			cboClienteID.Properties.ValueMember = "ID";
			cboClienteID.Properties.DisplayMember = "RagioneSociale";
			cboClienteID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
			cboClienteID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
			cboClienteID.Properties.BestFit();
		}

		private void FillTipoDocumenti()
		{
			var listItem = new List<EnTipoDocumento>();
			listItem.Add((EnTipoDocumento.NonSpecificato));
			listItem.Add(EnTipoDocumento.DDT);
			listItem.Add(EnTipoDocumento.FatturaDiCortesia);
			listItem.Add(EnTipoDocumento.RicevutaFiscale);

			listItem.Add(EnTipoDocumento.NotaDiCredito);

			cboTipoDocumento.DataSource = listItem.Select(a =>
					new
					{
						ID = a,
						Descrizione = UtilityView.GetTextSplitted( a.ToString())
					}
					).ToList();
			cboTipoDocumento.DisplayMember = "Descrizione";
			cboTipoDocumento.ValueMember = "ID";
		}

		private void FillPagamenti()
		{
			var listPagamenti = new List<Tuple<string, string>>();
			listPagamenti.Add(new Tuple<string, string>("", "Seleziona pagamento"));
			listPagamenti.Add(new Tuple<string, string>("Bonifico Bancario", "Bonifico Bancario"));
			listPagamenti.Add(new Tuple<string, string>("Rimessa Diretta", "Rimessa Diretta"));
			listPagamenti.Add(new Tuple<string, string>("CONTRASSEGNO CONTANTI", "CONTRASSEGNO CONTANTI"));

			cboPagamento.DataSource = listPagamenti.Select(a =>
					new
					{
						ID = a.Item1,
						Descrizione = a.Item2
					}
					).ToList();
			cboPagamento.DisplayMember = "Descrizione";
			cboPagamento.ValueMember = "ID";
		}

		private void TxtRagioneSociale_TextChanged(object sender, EventArgs e)
		{
			//using (var uof = new UnitOfWork())
			//{
			//	var finded = uof.ClientiRepository.Find(a => a.RagioneSociale == txtRagioneSociale.Text).Select(a => a.PIVA).FirstOrDefault();
			//	if (finded != null && finded.Length > 0)
			//	{
			//		txtPIVA.Text = finded;
			//	}
			//}
		}

		private async void frm_Load(object sender, EventArgs e)
		{
			await AddElemet();

			FillCombo();

			UpdateButtonState();

			UtilityView.SetDataBind(this, null, _controllerFatturazione.EditItem);

			cboClienteID.EditValueChanged += CboClienteID_EditValueChanged;

			txtRagioneSociale.TextChanged += TxtRagioneSociale_TextChanged;

			////calcolo del codice fattura
			if (_controllerFatturazione.EditItem.ID == 0)
			{
				cboTipoDocumento.SelectedValueChanged += (b, c) =>
				{
					try
					{
						_controllerFatturazione.EditItem.TipoDocumento = (EnTipoDocumento)cboTipoDocumento.SelectedValue;
					}
					catch (Exception)
					{


					}

					this.Validate();
					this.UpdateViewByTipoDocumento();
					//_controllerFatturazione.EditItem.TipoDocumento=
					if (txtCodice.Text == "")
					{
						var codice = _controllerFatturazione.CalcolaCodice();
						_controllerFatturazione.EditItem.Codice = codice;

						txtCodice.Text = codice;

					}
				};
			}
			else
			{
				cboTipoDocumento.Enabled = false;
			}

			using (var ord = new OrdinaTab())
			{
				ord.OrderTab(pnl1Alto);
				ord.OrderTab(pnl2Testo);
				ord.OrderTab(pnl3Basso);
			}
		}

		private void CboClienteID_EditValueChanged(object sender, EventArgs e)
		{
			var valCli = (int)cboClienteID.EditValue;
			using (var uof = new UnitOfWork())
			{
				var cliente = uof.ClientiRepository.Find(a => a.ID == valCli).FirstOrDefault();

				var item = (_controllerFatturazione.EditItem);
				item.RagioneSociale = cliente.RagioneSociale;
				item.PIVA = cliente.PIVA;

				Debug.WriteLine(cliente.RagioneSociale);
				this.Validate();
			}
		}

		private async Task AddElemet()
		{
			await Task.Run(() =>
			{
				_controllerRighe = new ControllerRigheFatture(_controllerFatturazione);
				var controlFattRigheList = new FattureRigheListView(_controllerRighe);

				controlFattRigheList.Height = 200;
				controlFattRigheList.Dock = DockStyle.Fill;
				tabPage2.Controls.Add(controlFattRigheList);

			});
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedTab == tabPage2)
			{
				if (((Fattura)_controllerFatturazione.EditItem).ID == 0)
				{
					tabControl1.SelectedTab = tabPage1;
					return;
				}
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
			if (_menuTab != null)
			{
				ribPannelRighe.Enabled = tabControl1.SelectedTab == tabPage2 &&
					_controllerFatturazione.SelectedItem.ID > 0;
				GetMenu().ApplyValidation(_controllerFatturazione.EditItem.ID > 0);
			}
			this.UpdateViewByTipoDocumento();
		}

		private MenuTab _menuTab = null;
		private RibbonMenuPanel ribPannelRighe = null;

		public event EventHandler<EventArgs> OnSave;
		public event EventHandler<EventArgs> OnClose;

		public void RaiseSave()
		{
			this.Validate();
			EventAggregator.Instance().Publish<Save<Fattura>>(
				new Save<Fattura>());

			txtID.Text = _controllerFatturazione.EditItem.ID.ToString();

			UpdateButtonState();
		}

		public void RaiseClose()
		{
			if (OnClose != null)
			{
				OnClose(this, new EventArgs());
			}
		}

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				var tab = _menuTab.Add("Principale");
				var ribPannel = tab.Add("Principale");
				UtilityView.AddButtonSaveAndClose(ribPannel, this);
				ribPannelRighe = tab.Add("Righe");
				ribPannelRighe.Add("Aggiungi", Properties.Resources.Add).Click += (a, b) =>
				{
					EventAggregator.Instance().Publish<Add<FatturaRiga>>(new Add<FatturaRiga>());
				};
				ribPannelRighe.Add("Rimuovi", Properties.Resources.Remove).Click
					+= (a, b) =>
					{
						EventAggregator.Instance().Publish<Remove<FatturaRiga>>(new Remove<FatturaRiga>());
					};

				ribPannelRighe.Add("Meno prioritario", Properties.Resources.Up).Click += (a, b) =>
				{
					EventAggregator.Instance().Publish<AddPriority<FatturaRiga>>(
						new AddPriority<FatturaRiga>());
				};
				ribPannelRighe.Add("Più prioritario", Properties.Resources.Down).Click
					+= (a, b) =>
					{
						EventAggregator.Instance().Publish<RemovePriority<FatturaRiga>>(
							new RemovePriority<FatturaRiga>());
					};


				_controllerFatturazione.AggiungiComandiStampa(tab, true);
				//var pnlStampa = tab.Add("Stampa");
				//var ribStampa = pnlStampa.Add("Avvia stampa", Properties.Resources.Print_48,true);
				//ribStampa.Click += (a, e) =>
				//{
				//	_controllerFatturazione.StampaFattura(_controllerFatturazione.EditItem);
				//};

				var pnl2 = tab.Add("Totali");
				var rib01 = pnl2.Add("Aggiorna totali", Properties.Resources.Totali_Aggiorna_48, true);
				rib01.Click += (a, e) =>
				{
					RebindEditItem();

				};
			}
			return _menuTab;
		}

		private void RebindEditItem()
		{
			Validate();
			_controllerFatturazione.EditItem = ControllerFatturazione.CalcolaTotali(_controllerFatturazione.EditItem);

			UtilityView.SetDataBind(this, null, _controllerFatturazione.EditItem);
		}
	}
}