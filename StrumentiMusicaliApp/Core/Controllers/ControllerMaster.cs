using NLog;
using NLog.Targets;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.View.DatiMittenteFattura;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core
{
	public class ControllerMaster : BaseController
	{
		private ControllerArticoli _controllerArticoli;
		private ControllerImmagini _controllerImmagini;
		private ControllerMagazzino _controllerMagazzino;
		private ControllerFatturazione _controllerFatturazione;

		public ControllerMaster()
			: base()
		{
			ConfigureNLog();
			_controllerArticoli = new ControllerArticoli();
			_controllerImmagini = new ControllerImmagini();
			_controllerMagazzino = new ControllerMagazzino();
			_controllerFatturazione = new ControllerFatturazione();

			EventAggregator.Instance().Subscribe<ApriAmbiente>(Apri);

			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			using (var mainView = new MainView(this))
			{
				this.ShowView(mainView, Settings.enAmbienti.Main);
			}

		}

		private void Apri(ApriAmbiente obj)
		{
			if (obj.TipoEnviroment==enTipoEnviroment.SettingFatture)
			{
				using (var view =new MittenteFatturaView())
				{
					this.ShowView(view,Settings.enAmbienti.SettingFatture);
				}
			}
		}

		~ControllerMaster()
		{
			_controllerFatturazione.Dispose();
			_controllerFatturazione = null;
			_controllerArticoli.Dispose();
			_controllerArticoli = null;
			_controllerImmagini.Dispose();
			_controllerImmagini = null;
			_controllerMagazzino.Dispose();
			_controllerMagazzino = null;
		}

		public static void LogMethod(string level, string message, string exception, string stacktrace, string classLine)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					uof.EventLogRepository.Add(new StrumentiMusicali.Library.Entity.EventLog()
					{ TipoEvento = level, Errore = message, TimeStamp = DateTime.Now, InnerException = exception, StackTrace = stacktrace, Class = classLine });

					uof.Commit();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(@"Errore nel salvataggio log\errore! " + Environment.NewLine + ex.Message, "info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void ConfigureNLog()
		{
			MethodCallTarget target = new MethodCallTarget();
			target.ClassName = typeof(ControllerMaster).AssemblyQualifiedName;
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
			_logger.Error(e.Exception, "Application_ThreadException", null);
		}
	}
}