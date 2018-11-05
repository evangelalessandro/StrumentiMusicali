using NLog;
using NLog.Targets;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core
{
	public class ControllerMaster : BaseController
	{
		 

		public ControllerMaster()
			: base()
		{
			ConfigureNLog();
			 

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
			switch (obj.TipoEnviroment)
			{

				case enAmbienti.ArticoliList:

					using (var controller = new ControllerArticoli())
					{

						using (var view = new ArticoliListView(controller))
						{
							this.ShowView(view, obj.TipoEnviroment);
						}
					}
					break;

				case enAmbienti.FattureList:
					using (var controller = new ControllerFatturazione())
					{
						using (var view = new FattureListView(controller, enAmbienti.FattureList, enAmbienti.Fattura))
						{
							ShowView(view, Settings.enAmbienti.FattureList,controller);
						}
					}
					break;
				case enAmbienti.LogView:
					using (var controller = new ControllerLog())
					{
						using (var view = new LogView(controller))
						{
							this.ShowView(view, obj.TipoEnviroment,controller);
						}
					}
					break;
				case enAmbienti.ClientiList:
					using (var controller = new ControllerClienti())
					{
						using (var view = new ClientiListView(controller))
						{
							this.ShowView(view, obj.TipoEnviroment, controller);
						}
					}
					break;
				case enAmbienti.SettingFatture:

					ApriSettingMittenteFattura();
					break;
				case enAmbienti.SettingSito:

					ApriSettingSito();
					break;
				case enAmbienti.SettingStampa:
					ApriSettingStampaFattura();
					break;
				case enAmbienti.Main:
					break;
				case enAmbienti.Fattura:
					break;
				case enAmbienti.Articolo:
					break;
				case enAmbienti.Magazzino:
					break;
				case enAmbienti.ScaricoMagazzino:
					using (var controller = new ControllerMagazzino())
					{
						using (var view = new View.ScaricoMagazzino(controller))
						{
							this.ShowView(view, Settings.enAmbienti.ScaricoMagazzino, controller);
						}
					}
					break;
				case enAmbienti.LogViewList:
					break;
				case enAmbienti.Cliente:
					break;
				case enAmbienti.FattureRigheList:
					break;
				case enAmbienti.FattureRigheDett:
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
				this.ShowView(view, enAmbienti.SettingFatture);
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
				this.ShowView(view, Settings.enAmbienti.SettingStampa);
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
				this.ShowView(view, Settings.enAmbienti.SettingSito);
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