using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerMagazzino : BaseController
	{
		public MovimentoMagazzino SelectedItem { get; set; } = new MovimentoMagazzino();

		public ControllerMagazzino()
			: base()
		{
			EventAggregator.Instance().Subscribe<ApriGestioneMagazzino>((a) =>
		   {
			   using (var frm = new Form())
			   { 
				   using (var view = new View.ScaricoMagazzino(this))
				   {
					   view.Dock = DockStyle.Fill;
					   frm.Controls.Add(view);
					   frm.ShowDialog();
				   }
			   }
		   });
			EventAggregator.Instance().Subscribe<ScaricaQtaMagazzino>(ScaricaMagazzino);
			EventAggregator.Instance().Subscribe<CaricaQtaMagazzino>(CaricaMagazzino);
		}

		internal System.Collections.Generic.List<DepositoItem> ListDepositi()
		{
			using (var uof = new UnitOfWork())
			{
				var listQtaDepositi = uof.MagazzinoRepository.Find(a => a.ArticoloID == SelectedItem.ArticoloID)
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
		}

		private void CaricaMagazzino(CaricaQtaMagazzino obj)
		{
			NuovoMovimento(new MovimentoMagazzino()
			{ ArticoloID = obj.ArticoloID, Deposito = obj.Deposito, Qta = obj.Qta });
		}

		private void NuovoMovimento(MovimentoMagazzino movimento)
		{
			try
			{
				using (var curs = new CursorManager())
				{
					using (var uof = new UnitOfWork())
					{
						if (movimento.Qta < 0)
						{
							var qtaDepositata = uof.MagazzinoRepository.Find(a => a.ArticoloID == movimento.ArticoloID &&
							a.DepositoID == movimento.Deposito).Select(a => a.Qta).DefaultIfEmpty(0).Sum();
							if (qtaDepositata < Math.Abs(movimento.Qta))
							{
								var depositoSel = uof.DepositoRepository.Find(a => a.ID == movimento.Deposito).First();
								MessageManager.NotificaWarnig(
									string.Format(
									@"La quantità presente nel deposito {0} è di {1} pezzi",
									depositoSel.NomeDeposito,
									qtaDepositata
									));
								return;
							}
						}

						uof.MagazzinoRepository.Add(new Library.Entity.Magazzino()
						{
							ArticoloID = movimento.ArticoloID,
							DepositoID = movimento.Deposito,
							Qta = (int)movimento.Qta
						});

						uof.Commit();
						MessageManager.NotificaInfo("Aggiunto movimento magazzino");
						EventAggregator.Instance().Publish<MovimentiUpdate>(new MovimentiUpdate());
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void ScaricaMagazzino(ScaricaQtaMagazzino obj)
		{
			NuovoMovimento(new MovimentoMagazzino()
			{ ArticoloID = obj.ArticoloID, Deposito = obj.Deposito, Qta = -obj.Qta });
		}
	}
}