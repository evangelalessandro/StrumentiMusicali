﻿using NLog;
using NLog.Targets;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core
{
	public class ControllerMaster :  BaseController
	{
		private ControllerArticoli _controllerArticoli;
		private ControllerImmagini _controllerImmagini;
		private ControllerMagazzino _controllerMagazzino;

		public ControllerMaster() 
			:base()
		{
			ConfigureNLog();
			_controllerArticoli = new ControllerArticoli();
			_controllerImmagini = new ControllerImmagini();
			_controllerMagazzino = new ControllerMagazzino();

			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new fmrMain(this));
		}
		~ControllerMaster()
		{
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