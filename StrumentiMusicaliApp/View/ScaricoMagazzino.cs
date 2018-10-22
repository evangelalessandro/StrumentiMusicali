using StrumentiMusicali.App.Core;
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
		private MovimentoMagazzino _item = new MovimentoMagazzino();
		public ScaricoMagazzino()
			: base()
		{
			InitializeComponent();
			lblTitoloArt.Text = "";
			ribScarica.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(new ScaricaQtaMagazzino()
				{
					Qta = _item.Qta,
					Deposito = _item.Deposito,
					ArticoloID = _item.ArticoloID
				});
			};
			ribCarica.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish<CaricaQtaMagazzino>(new CaricaQtaMagazzino()
				{
					Qta = _item.Qta,
					Deposito = _item.Deposito,
					ArticoloID = _item.ArticoloID
				});
			};
			cboDeposito.DisplayMember = "Descrizione";
			cboDeposito.ValueMember = "ID";
			txtQta.ValueChanged += (a, b) =>
			{
				_item.Qta = txtQta.Value;

				UpdateButton();
			};
			cboDeposito.SelectedValueChanged += (a, b) => { UpdateButton(); };
			txtQta.Tag = "Qta";
			cboDeposito.Tag = "Deposito";
			SetDataBind(this, _item);
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
					_item.ArticoloID = articolo.ID;
					lblTitoloArt.Text = articolo.Titolo;

					cboDeposito.DataSource = getListDepositi(uof);

					movimenti = uof.MagazzinoRepository.Find(a => a.ArticoloID == _item.ArticoloID)
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

		private System.Collections.Generic.List<DepositoItem> getListDepositi(UnitOfWork uof)
		{
			var listQtaDepositi = uof.MagazzinoRepository.Find(a => a.ArticoloID == _item.ArticoloID)
									.Select(
									a => new
									{
										ID = a.DepositoID,
										Qta = a.Qta
									}
									)
									.GroupBy(a => new { a.ID })
									.Select(g => new { ID = g.Key.ID, Qta = g.Sum(x => x.Qta) })
									.ToList();
			/*mette i depositi vuoti per quell'articolo*/
			var listDepositi = uof.DepositoRepository.Find(a => 1 == 1).ToList()
				.Distinct().Select(
				a => new DepositoItem()
				{
					ID = a.ID,
					NomeDeposito = a.NomeDeposito,
				}
				).ToList();

			foreach (var item in listDepositi)
			{
				var giac = listQtaDepositi.Where(b => b.ID == item.ID).FirstOrDefault();
				if (giac != null)
					item.Qta = giac.Qta;

			}


			listDepositi = listDepositi.Select(a => a).OrderBy(a => a.Descrizione).ToList();
			return listDepositi;
		}

		private async void ScaricoMagazzino_Load(object sender, EventArgs e)
		{
			await UpdateButton();
		}

		private async void txtCodiceABarre_TextChanged(object sender, EventArgs e)
		{
			_item.ArticoloID = "";
			lblTitoloArt.Text = "";
			RefreshData(new MovimentiUpdate());
			await UpdateButton();
		}

		private async Task UpdateButton()
		{
			try
			{
				var enableB = _item.ArticoloID != "" && _item.Qta > 0 && _item.Deposito > 0;
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
