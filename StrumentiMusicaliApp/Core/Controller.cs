using NLog;
using NLog.Targets;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Repo;
using StrumentiMusicaliApp.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Core
{
	public class Controller :IDisposable
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public Controller()
		{
			ConfigureNLog();

			EventAggregator.Instance().Subscribe<ArticoloAdd>(AggiungiArticolo);

			EventAggregator.Instance().Subscribe<ArticoloDuplicate>(DuplicaArticolo);
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new fmrMain());
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
			_logger.Error("Application_ThreadException",e.Exception);
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
			_logger.Info("Duplica articolo");
		}
	
	}
}
