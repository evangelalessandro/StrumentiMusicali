using DevExpress.XtraEditors.Repository;
using NLog;
using NLog.Targets;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Exports;
using StrumentiMusicali.App.Core.Imports;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Articoli;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Fatture;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Altro;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.RegistratoreDiCassa;
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

            EventAggregator.Instance().Subscribe<ApriAmbiente>(Apri);
            EventAggregator.Instance().Subscribe<ImportMagExcel>(ImportMagExcel);

            EventAggregator.Instance().Subscribe<ExportMagazzino>(ExportMag);

            Application.ThreadException += Application_ThreadException;
        }

        private void ExportMag(ExportMagazzino obj)
        {
            using (var mouseman = new CursorManager())
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


        public void ShowMainView()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!Debugger.IsAttached)
            {
                using (var login = new View.Login.LoginForm())
                {
                    login.TopMost = true;
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

                case enAmbiente.Scheduler:
                    var controllerSched = new ControllerScheduler();

                    var viewSched = new BaseGridViewGeneric<SchedulerItem, ControllerScheduler, SchedulerJob>(
                        controllerSched);

                    this.ShowView(viewSched, obj.TipoEnviroment, controllerSched);
                    viewSched.RicercaRefresh();
                    viewSched.dgvRighe.Columns["Errore"].ColumnEdit = new RepositoryItemMemoEdit();
                    var dateformat = new RepositoryItemDateEdit();
                    dateformat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    dateformat.DisplayFormat.FormatString = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.FullDateTimePattern;

                    viewSched.dgvRighe.Columns["UltimaEsecuzione"].ColumnEdit = dateformat;
                    viewSched.dgvRighe.Columns["ProssimoAvvio"].ColumnEdit = dateformat;

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
                case enAmbiente.MovimentiMagazzino:
                    var mag = new ControllerMovimentiMagazzino();

                    var viewMag = new BaseGridViewGeneric<MovimentoItemView, 
                        ControllerMovimentiMagazzino, Magazzino>(mag);


                    this.ShowView(viewMag, obj.TipoEnviroment, mag);
                    break;

                case enAmbiente.SettingFatture:

                    ApriSettingMittenteFattura();
                    break;

                case enAmbiente.SettingFtpBackup:

                    ApriSettingFtpBackup();
                    break;

                case enAmbiente.SettingScontrino:

                    ApriSettingScontrino();
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
                case enAmbiente.CategoriaArticoli:
                    {
                        var controllercat = new ControllerCategorie();

                        var viewCat = new BaseGridViewGeneric<CategoriaItem, ControllerCategorie, Categoria>(controllercat);

                        this.ShowView(viewCat, obj.TipoEnviroment, controllercat);
                    }
                    break;
                case enAmbiente.RegistratoreCassaReparti:
                    {
                        var controllercat = new ControllerRegistratoreDiCassaReparti();

                        var viewCat = new BaseGridViewGeneric<RegistratoreDiCassaRepartiItem, ControllerRegistratoreDiCassaReparti,
                            RegistratoreDiCassaReparti>(controllercat);

                        this.ShowView(viewCat, obj.TipoEnviroment, controllercat);
                    }

                    break;
                case enAmbiente.RegistratoreCassaGruppi:
                    {
                        var controllercat = new ControllerGruppoCodiceRegCassa();

                        var viewCat = new BaseGridViewGeneric<GruppoCodiceRegCassaItem, ControllerGruppoCodiceRegCassa, 
                            GruppoCodiceRegCassa>(controllercat);

                        this.ShowView(viewCat, obj.TipoEnviroment, controllercat);
                    }
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

        private void ApriSettingScontrino()
        {
            var setting = SettingScontrinoValidator.ReadSetting();

            var view = new GenericSettingView(setting);
            {
                view.OnSave += (a, b) =>
                {
                    using (var cur = new CursorManager())
                    {
                        view.Validate();
                        using (var save = new SaveEntityManager())
                        {
                            save.UnitOfWork.SettingScontrino.Update(setting);
                            save.SaveEntity(enSaveOperation.OpSave);
                        }
                    }
                };
                this.ShowView(view, enAmbiente.SettingSito);
            }

        }

        private void ApriSettingScheduler()
        {
            throw new NotImplementedException();
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



        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            _logger.Error(e.Exception, "Application_ThreadException", null);
        }
    }
}