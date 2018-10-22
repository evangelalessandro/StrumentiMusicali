using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.View
{
	public partial class ScaricoMagazzino : Base.BaseDataControl
	{
		ControllerMagazzino _controllerMagazzino;
		public ScaricoMagazzino(ControllerMagazzino controllerMagazzino)
			: base()
		{
			_controllerMagazzino = controllerMagazzino;
			InitializeComponent();
			lblTitoloArt.Text = "";
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
			cboDeposito.DisplayMember = "Descrizione";
			cboDeposito.ValueMember = "ID";
			txtQta.ValueChanged += (a, b) =>
			{
				_controllerMagazzino.SelectedItem.Qta = txtQta.Value;

				UpdateButton();
			};
			cboDeposito.SelectedValueChanged += (a, b) => { UpdateButton(); };
			txtQta.Tag = "Qta";
			cboDeposito.Tag = "Deposito";
			SetDataBind(this, _controllerMagazzino.SelectedItem);
			this.Load += ScaricoMagazzino_Load;

			EventAggregator.Instance().Subscribe<MovimentiUpdate>(RefreshData);

		}

		private void RefreshData(MovimentiUpdate obj)
		{
			var movimenti = new List<MovimentoItem>();
			using (var uof = new UnitOfWork())
			{
				var articolo = uof.ArticoliRepository.Find(a => a.CodiceAbarre == txtCodiceABarre.Text).FirstOrDefault();
				if (articolo != null)
				{
					_controllerMagazzino.SelectedItem.ArticoloID = articolo.ID;
					lblTitoloArt.Text = articolo.Titolo;

					cboDeposito.DataSource = _controllerMagazzino.ListDepositi();

					movimenti = uof.MagazzinoRepository.Find(a => a.ArticoloID == _controllerMagazzino.SelectedItem.ArticoloID)
						.Select(a => new MovimentoItem()
						{
							ID=a.ID,
							Articolo = a.Articolo.Titolo,
							Data = a.DataCreazione,
							NomeDeposito = a.Deposito.NomeDeposito,
							Qta = a.Qta
						})
						.ToList();
				}
				else
				{
					movimenti = uof.MagazzinoRepository.Find(a => 1 == 1)
						.OrderByDescending(a => a.DataUltimaModifica).Take(100)
						.Select(a => new MovimentoItem()
						{
							ID= a.ID,
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
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
	}
}
