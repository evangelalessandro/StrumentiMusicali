using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Settings;
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
	public partial class DettaglioFatturaView : UserControl, IMenu
	{
		private ControllerFatturazione _controllerFatturazione;

		private ControllerRigheFatture _controllerRighe;
		Subscription<RebindItemUpdated<Fattura>> _bindSub;
		public DettaglioFatturaView(ControllerFatturazione controllerFatturazione)
			: base()
		{
			_controllerFatturazione = controllerFatturazione;
			if (_controllerFatturazione is INotifyPropertyChanged)
			{
				(_controllerFatturazione as INotifyPropertyChanged).PropertyChanged += (a, b) =>
				{
					var prop = b.PropertyName;
					//if (prop=="SelectedItem")
					//	UtilityView.SetDataBind(this, _controllerFatturazione.SelectedItem);

				};
			}
			InitializeComponent();


			_bindSub =	EventAggregator.Instance().Subscribe<RebindItemUpdated<Fattura>>((a)=> { RebindEditItem(); });
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
			using (var uof = new UnitOfWork())
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

				txtAspettoBeni.Values = uof.FatturaRepository.Find(a => true).Select(a => a.AspettoEsterno).Distinct().ToList().ToArray();
				txtCausaleTrasporto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.CausaleTrasporto).Distinct().ToList().ToArray();
				txtPorto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Porto).Distinct().ToList().ToArray();
				txtTrasportoACura.Values = uof.FatturaRepository.Find(a => true).Select(a => a.TrasportoACura).Distinct().ToList().ToArray();
				txtVettore.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Vettore).Distinct().ToList().ToArray();
				txtNote1.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note1).Distinct().ToList().ToArray();
				txtNote2.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note2).Distinct().ToList().ToArray();

				cboClienteID.Properties.DataSource = uof.ClientiRepository.Find(a => true).Select(a => new { a.ID, a.RagioneSociale, a.PIVA }).Distinct().ToList().ToArray();
				cboClienteID.Properties.ValueMember = "ID";
				cboClienteID.Properties.DisplayMember = "RagioneSociale";
				cboClienteID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
				cboClienteID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
			}
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

			UtilityView.SetDataBind(this,null, _controllerFatturazione.EditItem);

			cboClienteID.EditValueChanged += CboClienteID_EditValueChanged;

			txtRagioneSociale.TextChanged += TxtRagioneSociale_TextChanged;

			using (var ord = new Utility.OrdinaTab())
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
		}

		private MenuTab _menuTab = null;
		private RibbonMenuPanel ribPannelRighe = null;

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				var tab = _menuTab.Add("Principale");
				var ribPannel = tab.Add("Principale");
				var ribSave = ribPannel.Add("Save", Properties.Resources.Save);
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

				ribSave.Click += (a, e) =>
				{
					this.Validate();
					EventAggregator.Instance().Publish<Save<Fattura>>(
						new Save<Fattura>());

					txtID.Text = _controllerFatturazione.SelectedItem.ID.ToString();
					UpdateButtonState();
				};

				var pnlStampa = tab.Add("Stampa");
				var ribStampa = pnlStampa.Add("Avvia stampa", Properties.Resources.Print_48,true);
				ribStampa.Click += (a, e) =>
				{
					_controllerFatturazione.StampaFattura(_controllerFatturazione.EditItem);
				};

				var pnl2 = tab.Add("Totali");
				var rib01 = pnl2.Add("Aggiorna totali", Properties.Resources.Totali_Aggiorna_48,true);
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