using NLog;
using NLog.Targets;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.Exports;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Enums;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerMaster : BaseController
	{
		 

		public ControllerMaster()
			: base()
		{
			ConfigureNLog();


			EventAggregator.Instance().Subscribe<ApriAmbiente>(Apri);
			EventAggregator.Instance().Subscribe<ImportArticoliCSVMercatino>(ImportaCsvArticoli);
			EventAggregator.Instance().Subscribe<ImportaFattureAccess>(ImportaFatture);
			EventAggregator.Instance().Subscribe<ImportArticoliMulino>(ImportArticoliMulinoExcel);
			EventAggregator.Instance().Subscribe<ImportMagExcel>(ImportMagExcel);

			EventAggregator.Instance().Subscribe<InvioArticoliCSV>(InvioArCSV);
			EventAggregator.Instance().Subscribe<ExportMagazzino>(ExportMag);

			

			Application.ThreadException += Application_ThreadException;
			

		}

		private void ExportMag(ExportMagazzino obj)
		{
			using (var export = new Controllers.Exports.ExportMagazzino())
			{
				export.Stampa();
			}
		}

		private void ImportMagExcel(ImportMagExcel obj)
		{
			using (var import = new Controllers.Imports.ImportMagazziniExcel())
			{
				import.ImportFile();
			}
		}
		private void ImportArticoliMulinoExcel(ImportArticoliMulino obj)
		{
			using (var import=new Controllers.Imports.ImportCavalloPazzoExcel())
			{
				import.ImportFile();
			}
		}

		public void ShowMainView()
		{
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			using (var mainView = new MainView(this))
			{
				this.ShowView(mainView, enAmbiente.Main);
			}
		}

		private void InvioArCSV(InvioArticoliCSV obj)
		{
			using (var controllerArt = new ControllerArticoli())
			{
				controllerArt.InvioArticoli(obj);
			}
		}

		private void ImportaFatture(ImportaFattureAccess obj)
		{
			using (var controller=new ControllerFatturazione())
			{
				controller.ImportaFatture(obj);
			}
		}

		private void ImportaCsvArticoli(ImportArticoliCSVMercatino obj)
		{
			using (var controllerArt=new ControllerArticoli())
			{
				controllerArt.ImportaCsvArticoli(null);
			}
		}

		private void Apri(ApriAmbiente obj)
		{
			switch (obj.TipoEnviroment)
			{

				case enAmbiente.ArticoliList:

					using (var controller = new ControllerArticoli())
					{

						using (var view = new ArticoliListView(controller))
						{
							this.ShowView(view, obj.TipoEnviroment,controller);
						}
					}
					break;

				case enAmbiente.FattureList:
					using (var controller = new ControllerFatturazione())
					{
						using (var view = new FattureListView(controller, enAmbiente.FattureList, enAmbiente.Fattura))
						{
							ShowView(view, enAmbiente.FattureList,controller);
						}
					}
					break;
				case enAmbiente.LogViewList:
					using (var controllerLog = new ControllerLog())
					{
						using (var view = new LogViewList(controllerLog))
						{
							this.ShowView(view, obj.TipoEnviroment, controllerLog);
						}
					}
					break;
				case enAmbiente.ClientiList:
					using (var controller = new ControllerClienti())
					{
						using (var view = new ClientiListView(controller))
						{
							this.ShowView(view, obj.TipoEnviroment, controller);
						}
					}
					break;
				case enAmbiente.DepositoList:
					using (var controller = new ControllerDepositi())
					{
						using (var view = new DepositiListView(controller))
						{
							this.ShowView(view, obj.TipoEnviroment, controller);
						}
					}
					break;
				case enAmbiente.SettingFatture:

					ApriSettingMittenteFattura();
					break;
				case enAmbiente.SettingSito:

					ApriSettingSito();
					break;
				case enAmbiente.SettingStampa:
					ApriSettingStampaFattura();
					break;
				case enAmbiente.Main:
					break;
				case enAmbiente.Fattura:
					break;
				case enAmbiente.Articolo:
					break;
				case enAmbiente.Magazzino:
					break;
				case enAmbiente.ScaricoMagazzino:
					using (var controller = new ControllerMagazzino())
					{
						using (var view = new View.ScaricoMagazzinoView(controller))
						{
							this.ShowView(view, enAmbiente.ScaricoMagazzino, controller);
						}
					}
					break;
				 
				case enAmbiente.Cliente:
					break;
				case enAmbiente.FattureRigheList:
					break;
				case enAmbiente.FattureRigheDett:
					break;
				default:
					break;
			}

		}
		private void ApriSettingMittenteFattura()
		{
			var setItem = this.ReadSetting().datiMittente;
			if (setItem == null)
			{
				setItem = new Controllers.FatturaElett.DatiMittente();
			}
			if (setItem.UfficioRegistroImp == null)
			{
				setItem.UfficioRegistroImp = new Controllers.FatturaElett.DatiMittente.UfficioRegistro();
			}
			if (setItem.Indirizzo == null)
			{
				setItem.Indirizzo = new Library.Entity.Indirizzo();
			}
			if (setItem.PecConfig == null)
			{
				setItem.PecConfig = new Library.Entity.PecConfig();
			}
			using (var view = new GenericSettingView(setItem))
			{
				view.OnSave += (a, b) =>
				{
					using (var cur = new CursorManager())
					{
						view.Validate();
						var setting = this.ReadSetting();
						setting.datiMittente = setItem;
						this.SaveSetting(setting);

						MessageManager.NotificaInfo(
							MessageManager.GetMessage(
								Core.Controllers.enSaveOperation.OpSave));
					}
				};
				this.ShowView(view, enAmbiente.SettingFatture);
			}
		}
		private void ApriSettingStampaFattura()
		{
			var setItem = this.ReadSetting().DatiIntestazione;
			if (setItem == null)
			{
				setItem = new DatiIntestazioneStampaFattura();
			}
			using (var view = new GenericSettingView(setItem))
			{
				view.OnSave += (a, b) =>
				{
					using (var cur = new CursorManager())
					{
						view.Validate();
						var setting = this.ReadSetting();
						setting.DatiIntestazione = setItem;
						this.SaveSetting(setting);

						MessageManager.NotificaInfo(
							MessageManager.GetMessage(
								Core.Controllers.enSaveOperation.OpSave));
					}
				};
				this.ShowView(view, enAmbiente.SettingStampa);
			}
		}

		private void ApriSettingSito()
		{
			SettingSito settingSito = this.ReadSetting().settingSito;
			if (settingSito == null)
			{
				settingSito = new SettingSito();
			}
			using (var view = new GenericSettingView(settingSito))
			{
				view.OnSave += (a, b) =>
				{
					using (var cur = new CursorManager())
					{
						view.Validate();
						var setting = this.ReadSetting();
						setting.settingSito = settingSito;
						this.SaveSetting(setting);

						MessageManager.NotificaInfo(
							MessageManager.GetMessage(
								Core.Controllers.enSaveOperation.OpSave));
					}
				};
				this.ShowView(view, enAmbiente.SettingSito);
			}
		}

		~ControllerMaster()
		{
		 
		}

		public static void LogMethod(string level, string message, string exception, string stacktrace, string classLine)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					uof.EventLogRepository.Add(new StrumentiMusicali.Library.Entity.EventLog()
					{ TipoEvento = level, Errore = message, DataCreazione = DateTime.Now, InnerException = exception, StackTrace = stacktrace, Class = classLine });

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