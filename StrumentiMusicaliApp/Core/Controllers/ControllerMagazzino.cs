using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerMagazzino : BaseController
	{
		public ControllerMagazzino()
			: base()
		{
			EventAggregator.Instance().Subscribe<ApriGestioneMagazzino>((a) =>
		   {
			   using (var view = new View.ScaricoMagazzino())
			   {
				   view.ShowDialog();
			   }
		   });
			EventAggregator.Instance().Subscribe<ScaricaQtaMagazzino>(ScaricaMagazzino);
			EventAggregator.Instance().Subscribe<CaricaQtaMagazzino>(CaricaMagazzino);
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

				using (var curs = new CursorHandler())
				{
					using (var uof = new UnitOfWork())
					{
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
