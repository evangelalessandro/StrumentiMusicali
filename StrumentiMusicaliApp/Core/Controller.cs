using NLog;
using NLog.Targets;
using StrumentiMusicaliApp.Core.Events;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Repo;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Core
{
	public class Controller : IDisposable
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public Controller()
		{
			ConfigureNLog();

			EventAggregator.Instance().Subscribe<ArticoloAdd>(AggiungiArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDelete>(DeleteArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDuplicate>(DuplicaArticolo);
			EventAggregator.Instance().Subscribe<ArticoloSelected>(ArticoloSelectedChange);
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new fmrMain());
		}

		private void DeleteArticolo(ArticoloDelete obj)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					
					var item= uof.ArticoliRepository.Find(a => a.ID == obj.ItemSelected.ID).FirstOrDefault();
					_logger.Info(string.Format("Cancellazione articolo /r/n{0} /r/n{1}", item.Titolo,item.ID));
					uof.ArticoliRepository.Delete(item);
					uof.Commit();
				}
				MessageManager.NotificaInfo("Cancellazione avvenuta correttamente!");
				EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private ArticoloSelected _ArticoloSelected;
		private void ArticoloSelectedChange(ArticoloSelected obj)
		{
			_ArticoloSelected = obj;
		}

		public static void LogMethod(string level, string message, string exception, string stacktrace, string classLine)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					uof.EventLogRepository.Add(new StrumentiMusicaliSql.Entity.EventLog()
					{ TipoEvento = level, Errore = message, TimeStamp = DateTime.Now, InnerException = exception, StackTrace = stacktrace, Class = classLine });
					uof.Commit();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(@"Errore nel salvataggio log\errore! " + Environment.NewLine + ex.Message, "info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		private static void ConfigureNLog()
		{
			MethodCallTarget target = new MethodCallTarget();
			target.ClassName = typeof(Controller).AssemblyQualifiedName;
			target.MethodName = "LogMethod";
			target.Parameters.Add(new MethodCallParameter("${level}"));
			target.Parameters.Add(new MethodCallParameter("${message}"));
			target.Parameters.Add(new MethodCallParameter("${exception:format=tostring,Data:maxInnerExceptionLevel=10}"));
			target.Parameters.Add(new MethodCallParameter("${stacktrace}"));
			target.Parameters.Add(new MethodCallParameter("${callsite}"));


			NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
		}

		private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			_logger.Error("Application_ThreadException", e.Exception);
		}

		public void Dispose()
		{

		}

		private void AggiungiArticolo(ArticoloAdd articoloAdd)
		{
			_logger.Info("AggiungiArticolo");

			using (var frm = new Forms.frmArticolo())
			{
				frm.ShowDialog();
			}
		}

		private void DuplicaArticolo(ArticoloDuplicate obj)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					var itemCurrent = _ArticoloSelected.ItemSelected.ArticoloCS;
					uof.ArticoliRepository.Add(new StrumentiMusicaliSql.Entity.Articolo()
					{
						Categoria = itemCurrent.Categoria,
						Condizione = itemCurrent.Condizione,
						BoxProposte = itemCurrent.BoxProposte,
						Giacenza = itemCurrent.Giacenza,
						Marca = itemCurrent.Marca,
						Prezzo = itemCurrent.Prezzo,
						PrezzoARichiesta = itemCurrent.PrezzoARichiesta,
						PrezzoBarrato = itemCurrent.PrezzoBarrato,
						Testo = itemCurrent.Testo,
						Titolo = itemCurrent.Titolo,
						UsaAnnuncioTurbo = itemCurrent.UsaAnnuncioTurbo,
						DataUltimaModifica = DateTime.Now
					});
					uof.Commit();
				}
				MessageManager.NotificaInfo("Duplicazione avvenuta co successo");
				_logger.Info("Duplica articolo");
				EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());

				
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		
	}
}
