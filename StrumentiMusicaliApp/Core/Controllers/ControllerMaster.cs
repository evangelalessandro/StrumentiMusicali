using NLog;
using NLog.Targets;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Exports;
using StrumentiMusicali.App.Core.Imports;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Articoli;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Fatture;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Diagnostics;
using System.Linq;
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
            using (var mouseman = new Manager.CursorManager())
            {
                using (var export = new ExportMagazzino())
                {
                    export.TipoExp = obj.TipoExp;
                    export.Stampa();
                }
            }
        }

        private void ImportMagExcel(ImportMagExcel obj)
        {
            using (var import = new ImportMagazziniExcel())
            {
                import.ImportFile();
            }
        }
        private void ImportArticoliMulinoExcel(ImportArticoliMulino obj)
        {
            using (var import = new ImportCavalloPazzoExcel())
            {
                import.ImportFile();
            }
        }

        public void ShowMainView()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!Debugger.IsAttached)
            {
                using (var login = new View.Login.LoginForm())
                {
                    var ret = login.ShowDialog();
                    if (ret != DialogResult.OK)
                        return;
                }
            }
            else
            {
                using (var uof = new UnitOfWork())
                {
                    var utente = uof.UtentiRepository.Find(a => a.NomeUtente == "Admin").FirstOrDefault();

                    if (utente != null)
                    {
                        LoginData.SetUtente(utente);
                    }
                }
            }
            using (var mainView = new MainView(this))
            {
                mainView.Load += MainView_Load;
                mainView.Disposed += MainView_Disposed;

                this.ShowView(mainView, enAmbiente.Main);


            }
        }

        private void MainView_Disposed(object sender, EventArgs e)
        {
            if ((sender as UserControl).IsDisposed)
            {
                return;
            }
            if (!SettingBackupFtpValidator.ScadutoTempoBackup())
            {
                return;
            }
            var res = MessageManager.QuestionMessage("Vuoi effettuare il backup del database?");
            if (res)
            {
                try
                {
                    using (var uof = new UnitOfWork())
                    {
                        uof.EseguiBackup();

                        using (var ftpManager = new ftpBackup.Backup.BackupManager())
                        {
                            ftpManager.Manage();

                            MessageBox.Show("Backup Effettuato correttamente", "Info", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageManager.NotificaError("Errore nella fase backup in chiusura", ex);

                }
            }
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            /*controlla se è stato fatto il backup*/
            CheckBackup();
        }

        private void CheckBackup()
        {
            var setting = SettingBackupFtpValidator.ReadSetting();
            if (SettingBackupFtpValidator.ScadutoTempoBackup())
            {
                MessageManager.NotificaWarnig("Non sono stati effettuati backup successivi all'ultimo del " + setting.UltimoBackup.ToString());
            }

        }

        private void InvioArCSV(InvioArticoliCSV obj)
        {
            using (var controllerArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.Tutto))
            {
                controllerArt.InvioArticoli(obj);
            }
        }

        private void ImportaFatture(ImportaFattureAccess obj)
        {
            using (var controller = new ControllerFatturazione())
            {
                controller.ImportaFatture(obj);
            }
        }

        private void ImportaCsvArticoli(ImportArticoliCSVMercatino obj)
        {
            using (var controllerArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.Tutto))
            {
                controllerArt.ImportaCsvArticoli(null);
            }
        }

        private void Apri(ApriAmbiente obj)
        {
            switch (obj.TipoEnviroment)
            {

                case enAmbiente.StrumentiList:
                    {
                        var controllerArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.SoloStrumenti);
                        var viewArt = new ArticoliListView(controllerArt);
                        this.ShowView(viewArt, obj.TipoEnviroment, controllerArt);
                        break;
                    }
                case enAmbiente.LibriList:
                    {
                        var controllerArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.SoloLibri);
                        var viewArt = new ArticoliListView(controllerArt);
                        this.ShowView(viewArt, obj.TipoEnviroment, controllerArt);
                        break;
                    }
                case enAmbiente.RicercaArticolo:

                    var contrArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.Ricerca);
                    var viewRicercaArt = new RicercaArticoliStyleView(contrArt);

                    this.ShowView(viewRicercaArt, obj.TipoEnviroment, contrArt);


                    break;
                case enAmbiente.PagamentiList:

                    var controllerPagamenti = new ControllerPagamenti();
                    var viewPagamenti = new PagamentiListView(controllerPagamenti);
                    this.ShowView(viewPagamenti, obj.TipoEnviroment, controllerPagamenti);


                    break;
                case enAmbiente.UtentiList:

                    var controllerUtenti = new ControllerUtenti();


                    var viewUtenti = new UtentiListView(controllerUtenti);

                    this.ShowView(viewUtenti, obj.TipoEnviroment, controllerUtenti);


                    break;

                case enAmbiente.FattureList:
                    var controllerFatt = new ControllerFatturazione();

                    var viewFatt = new FattureListView(controllerFatt, enAmbiente.FattureList, enAmbiente.Fattura);

                    ShowView(viewFatt, enAmbiente.FattureList, controllerFatt);

                    break;
                case enAmbiente.LogViewList:
                    var controllerLog = new ControllerLog();

                    var viewLog = new LogViewList(controllerLog);

                    this.ShowView(viewLog, obj.TipoEnviroment, controllerLog);


                    break;
                case enAmbiente.ClientiList:
                    var controllerClienti = new ControllerClienti();

                    var viewCli = new ClientiListView(controllerClienti);

                    this.ShowView(viewCli, obj.TipoEnviroment, controllerClienti);
                    break;
                case enAmbiente.DepositoList:
                    var controllerDep = new ControllerDepositi();

                    var viewDep = new DepositiListView(controllerDep);

                    this.ShowView(viewDep, obj.TipoEnviroment, controllerDep);
                    break;
                case enAmbiente.SettingFatture:

                    ApriSettingMittenteFattura();
                    break;
                case enAmbiente.SettingFtpBackup:

                    ApriSettingFtpBackup();
                    break;
                case enAmbiente.SettingSito:

                    ApriSettingSito();
                    break;
                case enAmbiente.SettingDocPagamenti:

                    ApriSettingDocPagamenti();
                    break;
                case enAmbiente.SettingStampa:
                    ApriSettingStampaFattura();
                    break;
                case enAmbiente.Main:
                    break;
                case enAmbiente.Fattura:
                    break;
                case enAmbiente.StrumentiDetail:
                    break;
                case enAmbiente.Magazzino:
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

        private void ApriSettingFtpBackup()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {

                var setItem = SettingBackupFtpValidator.ReadSetting();


                var view = new GenericSettingView(setItem);
                {
                    view.OnSave += (a, b) =>
                    {
                        using (var cur = new CursorManager())
                        {
                            view.Validate();
                            using (var save = new SaveEntityManager())
                            {
                                save.UnitOfWork.SettingBackupFtpRepository.Update(setItem);
                                save.SaveEntity(enSaveOperation.OpSave);
                            }
                        }
                    };
                    this.ShowView(view, enAmbiente.SettingFtpBackup);
                }
            }
        }

        private void ApriSettingDocPagamenti()
        {
            var setting = SettingDocumentiPagamentiValidator.ReadSetting();

            var view = new GenericSettingView(setting);
            {
                view.OnSave += (a, b) =>
                {
                    using (var cur = new CursorManager())
                    {
                        view.Validate();
                        using (var save = new SaveEntityManager())
                        {
                            save.UnitOfWork.SettingDocumentiPagamentiRepository.Update(setting);
                            save.SaveEntity(enSaveOperation.OpSave);
                        }
                    }
                };
                this.ShowView(view, enAmbiente.SettingSito);
            }
        }

        private void ApriSettingMittenteFattura()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var setItem = unitOfWork.DatiMittenteRepository.Find(a => 1 == 1).FirstOrDefault();

                if (setItem == null)
                {
                    setItem = new DatiMittente();
                    setItem.ID = 1;
                    unitOfWork.DatiMittenteRepository.Add(setItem);
                    unitOfWork.Commit();
                }
                if (setItem.UfficioRegistroImp == null)
                {
                    setItem.UfficioRegistroImp = new DatiMittente.UfficioRegistro();
                }
                if (setItem.Indirizzo == null)
                {
                    setItem.Indirizzo = new Library.Entity.Indirizzo();
                }
                if (setItem.PecConfig == null)
                {
                    setItem.PecConfig = new Library.Entity.PecConfig();
                }

                var view = new GenericSettingView(setItem);
                {
                    view.OnSave += (a, b) =>
                    {
                        using (var cur = new CursorManager())
                        {
                            view.Validate();
                            using (var save = new SaveEntityManager())
                            {
                                save.UnitOfWork.DatiMittenteRepository.Update(setItem);
                                save.SaveEntity(enSaveOperation.OpSave);
                            }
                        }
                    };
                    this.ShowView(view, enAmbiente.SettingFatture);
                }
            }
        }
        private void ApriSettingStampaFattura()
        {
            var setItem = DatiIntestazioneStampaFatturaValidator.ReadSetting();

            var view = new GenericSettingView(setItem);
            {
                view.OnSave += (a, b) =>
                {
                    using (var cur = new CursorManager())
                    {
                        view.Validate();
                        using (var uof = new SaveEntityManager())
                        {
                            uof.UnitOfWork.DatiIntestazioneStampaFatturaRepository.Update(setItem);
                            uof.SaveEntity(enSaveOperation.OpSave);
                        }


                    }
                };
                this.ShowView(view, enAmbiente.SettingStampa);
            }
        }

        private void ApriSettingSito()
        {
            var settingSito = SettingSitoValidator.ReadSetting();

            var view = new GenericSettingView(settingSito);
            {
                view.OnSave += (a, b) =>
                {
                    using (var cur = new CursorManager())
                    {
                        view.Validate();
                        using (var save = new SaveEntityManager())
                        {
                            save.UnitOfWork.SettingSitoRepository.Update(settingSito);
                            save.SaveEntity(enSaveOperation.OpSave);
                        }
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