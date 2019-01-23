using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StrumentiMusicali.App.View
{
	public partial class ScaricoMagazzinoView : BaseDataControl
	{
		private ControllerMagazzino _controllerMagazzino;

		~ScaricoMagazzinoView()
		{
			EventAggregator.Instance().UnSbscribe(_selectItem);
		}

		public ScaricoMagazzinoView(ControllerMagazzino controllerMagazzino)
			: base()
		{

			_controllerMagazzino = controllerMagazzino;
			_controllerMagazzino.SelectedItem = new MovimentoMagazzino();
			_controllerMagazzino.SelectedItem.Qta = 1;
			InitializeComponent();
			lblTitoloArt.Text = "";
			UtilityView.InitGridDev(dgvRighe);

			listDepositi.DisplayMember = "Descrizione";
			listDepositi.ValueMember = "ID";
			txtQta.ValueChanged += (a, b) =>
			{
				_controllerMagazzino.SelectedItem.Qta = txtQta.Value;

				UpdateButton();
			};
			listDepositi.SelectedValueChanged += (a, b) =>
			{
				try
				{
					_controllerMagazzino.SelectedItem.Deposito =
						((DepositoScaricoItem)listDepositi.SelectedItem).ID;
				}
				catch (Exception ex)
				{
				}

				UpdateButton();
			};
			txtQta.Tag = "Qta";
			listDepositi.Tag = "Deposito";


			UtilityView.SetDataBind(this, null, _controllerMagazzino.SelectedItem);
			this.Load += ScaricoMagazzino_Load;
			this.Load += ScaricoMagazzino_LoadSync;

			EventAggregator.Instance().Subscribe<MovimentiUpdate>(RefreshData);
			EventAggregator.Instance().Subscribe<ValidateViewEvent<Magazzino>>(
				(a) => { this.Validate(); }
				);

			if (_controllerMagazzino.ArticoloFilter != null)
			{
				txtCodiceABarre.Text = "";
				_cboArticoli.Controllo.EditValue = _controllerMagazzino.ArticoloFilter.ID;
				_cboArticoli.Visible = false;
				txtCodiceABarre.Visible = false;
				pnlTop.Visible = false;
			}
		}

		private Subscription<MagazzinoSelezionaArticolo> _selectItem;


		private void ScaricoMagazzino_LoadSync(object sender, EventArgs e)
		{
			LoadListArticoli();

			 
		}

		private void RefreshData(MovimentiUpdate obj)
		{
			lblTitoloArticolo.ForeColor = System.Drawing.Color.Red;
			lblGiacenzaArticolo.Text = "-";
			var movimenti = new List<MovimentoItem>();
			using (var uof = new UnitOfWork())
			{
				var articolo = uof.ArticoliRepository.Find(a => a.CodiceABarre == txtCodiceABarre.Text && txtCodiceABarre.Text.Length > 0).FirstOrDefault();
				if (articolo == null)
				{
					if (_cboArticoli.Controllo != null && _cboArticoli.Controllo.EditValue != null)
					{
						var val = int.Parse(_cboArticoli.Controllo.EditValue.ToString());
						articolo = uof.ArticoliRepository.Find(a => a.ID == val).FirstOrDefault();
					}
				}
				if (articolo != null)
				{
					lblTitoloArticolo.ForeColor = System.Drawing.Color.Green;
					var depoSel = listDepositi.SelectedIndex;
					_controllerMagazzino.SelectedItem.ArticoloID = articolo.ID;
					lblTitoloArt.Text = " {" +articolo.ID.ToString() + "} " +  articolo.Titolo;

					var listDepo = _controllerMagazzino.ListDepositi();
					listDepositi.DataSource = listDepo;
					if (depoSel == -1 && listDepo.Count > 0)
					{
						listDepositi.SelectedIndex = 0;
					}
					else
					{
						listDepositi.SelectedIndex = depoSel;
					}
					movimenti = uof.MagazzinoRepository.Find(a => a.ArticoloID == _controllerMagazzino.SelectedItem.ArticoloID)
						.Select(a => new MovimentoItem()
						{
							ID = a.ID,
							Articolo = a.Articolo.Titolo,
							Data = a.DataCreazione,
							NomeDeposito = a.Deposito.NomeDeposito,
							Qta = a.Qta
						})
						.OrderByDescending(a => a.ID)
						.ToList();

					lblGiacenzaArticolo.Text = movimenti.Select(a => a.Qta).Sum().ToString();
				}
				else
				{
					listDepositi.DataSource = new List<DepositoItem>();
					movimenti = uof.MagazzinoRepository.Find(a => 1 == 1)
						.OrderByDescending(a => a.ID).Take(100)
						.Select(a => new MovimentoItem()
						{
							ID = a.ID,
							Articolo = a.Articolo.Titolo,
							Data = a.DataCreazione,
							NomeDeposito = a.Deposito.NomeDeposito,
							Qta = a.Qta
						})
						.ToList();
				}

				gridControl1.DataSource = movimenti;
			}
			dgvRighe.Columns["Data"].DisplayFormat.FormatString = "G";

		}

		private EDCombo _cboArticoli = new EDCombo();

		private async void ScaricoMagazzino_Load(object sender, EventArgs e)
		{
			UpdateButton();
			RefreshData(new MovimentiUpdate());

		}

		private void LoadListArticoli()
		{
			pnlTop.Controls.Add(_cboArticoli);
			_cboArticoli.Top = 0;
			_cboArticoli.Font = this.Font;
			_cboArticoli.Left = txtCodiceABarre.Left + txtCodiceABarre.Width + 10;
			_cboArticoli.Width = pnlMidle.Width - _cboArticoli.Left - 20;
			_cboArticoli.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
			_cboArticoli.Controllo.Properties.PopupWidth = pnlTop.Width;
			using (var uof = new UnitOfWork())
			{
				var list = uof.ArticoliRepository.Find(a => true)
					.Select(a => new
					{
						a.ID,
						Descrizione = a.Titolo + " " + a.Testo + " " + a.Categoria.Nome,
						Categoria = a.Categoria.Nome,
						a.Categoria.Reparto
					}
					).ToList();
				_cboArticoli.SetList(list);
				_cboArticoli.Controllo.Properties.NullText = "<Nessun Articolo selezionato>";
				_cboArticoli.Titolo = "Ricerca per articolo";
				_cboArticoli.Controllo.EditValueChanged += Controllo_EditValueChanged;
			}
		}

		private void Controllo_EditValueChanged(object sender, EventArgs e)
		{
			RefreshData(new MovimentiUpdate());
		}

		private async void txtCodiceABarre_TextChanged(object sender, EventArgs e)
		{
			_controllerMagazzino.SelectedItem.ArticoloID = 0;
			lblTitoloArt.Text = "";
			RefreshData(new MovimentiUpdate());
			UpdateButton();
		}

		private void UpdateButton()
		{
			try
			{

				var enableB = _controllerMagazzino.SelectedItem.ArticoloID != 0
					&& _controllerMagazzino.SelectedItem.Qta > 0 && _controllerMagazzino.SelectedItem.Deposito > 0;

				foreach (var item in _controllerMagazzino.GetMenu().ItemByTag(ControllerMagazzino.TAG_CARICA))
				{
					item.Enabled = enableB;
				}
				foreach (var item in _controllerMagazzino.GetMenu().ItemByTag(ControllerMagazzino.TAG_SCARICA))
				{
					item.Enabled = enableB;
				}
				this.Validate();

			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
		
	}
}