using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.View
{
	public partial class ScaricoMagazzino : BaseDataControl, IMenu
	{
		private ControllerMagazzino _controllerMagazzino;

		public ScaricoMagazzino(ControllerMagazzino controllerMagazzino)
			: base()
		{
			_controllerMagazzino = controllerMagazzino;
			_controllerMagazzino.SelectedItem = new MovimentoMagazzino();
			_controllerMagazzino.SelectedItem.Qta = 1;
			InitializeComponent();
			lblTitoloArt.Text = "";
		 
			cboDeposito.DisplayMember = "Descrizione";
			cboDeposito.ValueMember = "ID";
			txtQta.ValueChanged += (a, b) =>
			{
				_controllerMagazzino.SelectedItem.Qta = txtQta.Value;

				UpdateButton();
			};
			cboDeposito.SelectedValueChanged += (a, b) =>
			{
				try
				{
					_controllerMagazzino.SelectedItem.Deposito =
						((DepositoItem)cboDeposito.SelectedItem).ID;
				}
				catch (Exception)
				{
				}

				UpdateButton();
			};
			txtQta.Tag = "Qta";
			cboDeposito.Tag = "Deposito";
			UtilityView.SetDataBind(this, _controllerMagazzino.SelectedItem);
			this.Load += ScaricoMagazzino_Load;

			EventAggregator.Instance().Subscribe<MovimentiUpdate>(RefreshData);
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

		private MenuTab _menuTab = null;
		private void AggiungiComandi()
		{
			var tabArticoli = _menuTab.Add("Principale");
			var panelComandiArticoli = tabArticoli.Add("Comandi");
			ribCarica = panelComandiArticoli.Add("Carica", Properties.Resources.Add);
			ribCarica.Click += (a, e) =>
			{
				this.Validate();
				EventAggregator.Instance().Publish<CaricaQtaMagazzino>(new CaricaQtaMagazzino()
				{
					Qta = _controllerMagazzino.SelectedItem.Qta,
					Deposito = _controllerMagazzino.SelectedItem.Deposito,
					ArticoloID = _controllerMagazzino.SelectedItem.ArticoloID
				});
			};

			ribScarica = panelComandiArticoli.Add("Carica", Properties.Resources.Remove);
			ribScarica.Click += (a, e) =>
			{
				this.Validate();
				EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(new ScaricaQtaMagazzino()
				{
					Qta = _controllerMagazzino.SelectedItem.Qta,
					Deposito = _controllerMagazzino.SelectedItem.Deposito,
					ArticoloID = _controllerMagazzino.SelectedItem.ArticoloID
				});
			};

		}
		RibbonMenuButton ribCarica;
		RibbonMenuButton ribScarica;
		private void RefreshData(MovimentiUpdate obj)
		{
			lblTitoloArticolo.ForeColor = System.Drawing.Color.Red;
			var movimenti = new List<MovimentoItem>();
			using (var uof = new UnitOfWork())
			{
				var articolo = uof.ArticoliRepository.Find(a => a.CodiceAbarre == txtCodiceABarre.Text).FirstOrDefault();
				if (articolo != null)
				{
					lblTitoloArticolo.ForeColor = System.Drawing.Color.Green;
					var depoSel = cboDeposito.SelectedIndex;
					_controllerMagazzino.SelectedItem.ArticoloID = articolo.ID;
					lblTitoloArt.Text = articolo.Titolo;

					var listDepo = _controllerMagazzino.ListDepositi();
					cboDeposito.DataSource = listDepo;
					if (depoSel == -1 && listDepo.Count > 0)
					{
						cboDeposito.SelectedIndex = 0;
					}
					else
					{
						cboDeposito.SelectedIndex = depoSel;
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
				}
				else
				{
					cboDeposito.DataSource = new List<DepositoItem>();
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

				dgvMaster.DataSource = movimenti;
			}
		}

		private async void ScaricoMagazzino_Load(object sender, EventArgs e)
		{
			await UpdateButton();
			RefreshData(new MovimentiUpdate());
		}

		private async void txtCodiceABarre_TextChanged(object sender, EventArgs e)
		{
			_controllerMagazzino.SelectedItem.ArticoloID = "";
			lblTitoloArt.Text = "";
			RefreshData(new MovimentiUpdate());
			await UpdateButton();
		}

		private async Task UpdateButton()
		{
			try
			{
				var enableB = _controllerMagazzino.SelectedItem.ArticoloID != "" && _controllerMagazzino.SelectedItem.Qta > 0 && _controllerMagazzino.SelectedItem.Deposito > 0;
				ribCarica.Enabled = enableB;
				ribScarica.Enabled = enableB;
				this.Validate();
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
	}
}