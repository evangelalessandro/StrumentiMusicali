using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.View.Base;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
				var listPagamenti = new List<Tuple<int, string>>();
				listPagamenti.Add(new Tuple<int, string>(-1, "Seleziona pagamento"));
				listPagamenti.Add(new Tuple<int, string>(1, "Bonifico Bancario"));
				listPagamenti.Add(new Tuple<int, string>(2, "Rimessa Diretta"));

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
			}
		}

		private void frmArticolo_Load(object sender, EventArgs e)
		{
			FillCombo();

			UpdateButtonState();

			SetDataBind(this, _controllerFatturazione.SelectedItem);
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
		}

		private void UpdateButtonState()
		{
			ribPanelRighe.Enabled = tabControl1.SelectedTab == tabPage2;
			ribRemove.Enabled = true;
		}
	}
}