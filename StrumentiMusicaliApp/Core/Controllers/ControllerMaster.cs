using DevExpress.UserSkins;
using DevExpress.XtraEditors.Repository;
using StrumentiMusicali.App.Core.Controllers.Articoli;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.ListiniFornitori;
using StrumentiMusicali.App.Core.Exports;
using StrumentiMusicali.App.Core.Imports;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Articoli;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Core.Enum;
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
using StrumentiMusicali.Library.Entity.Fatture;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerMaster : BaseController, IMenu
    {
        private MenuTab _menuTab = null;
        private SettingProgramma _settingProgramma = StrumentiMusicali.Core.Settings.SettingProgrammaValidator.ReadSetting();

        public ControllerMaster()
            : base()
        {

            EventAggregator.Instance().Subscribe<ApriAmbiente>(Apri);
            EventAggregator.Instance().Subscribe<ImportaFattureAccess>(ImportaFatture);
            EventAggregator.Instance().Subscribe<ImportArticoliMulino>(ImportArticoliMulinoExcel);
            EventAggregator.Instance().Subscribe<ImportMagExcel>(ImportMagExcel);

            EventAggregator.Instance().Subscribe<ExportMagazzino>(ExportMag);

            Application.ThreadException += Application_ThreadException;

        }
        ~ControllerMaster()
        {
        }

        public MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                AggiungiPrincipale();

                AggiungiImpostazioni();

                AggiungiGestione();

                AggiungiTabLog();

                AggiungiComandiImportExport();
            }
            return _menuTab;
        }

        public void ShowMainView()
        {
            BonusSkins.Register();
            //    DevExpress.Skins.SkinManager.EnableFormSkins();
            //UserLookAndFeel.Default.SetSkinStyle(SkinStyle.Foggy);

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

                this.ShowView(mainView, enAmbiente.Main, this);
            }
        }

        private static void AggiungiPulsanteSetting(RibbonMenuPanel panel1, string nome, enAmbiente ambiente, System.Drawing.Bitmap image = null)
        {
            if (image == null)
                image = StrumentiMusicali.Core.Properties.ImageIcons.Settings;
            panel1.Add(nome, image)
                            .Click += (s, e) =>
                            {
                                EventAggregator.Instance().Publish(new ApriAmbiente(ambiente));
                            };
        }

        private void AggiungiComandiImportExport()
        {
            var tabImportExport = _menuTab.Add(@"Import\Export");
            var pnlExport = tabImportExport.Add("Export");
            if (_settingProgramma.GestioneLibri)
            {
                pnlExport.Add("Export Tutti libri Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export).Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.TuttoLibri });
                };
            }
            pnlExport.Add("Export Tutti " + _settingProgramma.NomeAnagrafica + "  Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export).Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.TuttoStrumenti });
            };
            var pnlExport2 = tabImportExport.Add("Elenco Mancanti");
            if (_settingProgramma.GestioneLibri)
            {
                pnlExport2.Add("Libri(Betta)", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export).Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.SoloLibriMancanti });
                };
            }
            pnlExport2.Add("X marca (Luca)", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export).Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.PerMarca });
            };


        }

        private void AggiungiGestione()
        {
            var tabImportExport = _menuTab.Add(@"Gestione");
            var panel1 = tabImportExport.Add("Principale");

            panel1.Add("SottoScorta",
                StrumentiMusicali.Core.Properties.ImageIcons.Reorder_48).Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ArticoliSottoscorta));
                };


        }

        private void AggiungiImpostazioni()
        {
            RibbonMenuTab tabSetting = _menuTab.Add(@"Impostazioni");
            var panel1 = tabSetting.Add("Principale");

            var rib2 = panel1.Add("Sito & Upload", StrumentiMusicali.Core.Properties.ImageIcons.Settings);
            rib2.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingSito));
            };



            var rib4 = panel1.Add("Depositi", StrumentiMusicali.Core.Properties.ImageIcons.Settings);
            rib4.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.DepositoList));
            };

            var rib5 = panel1.Add("Backup ftp", StrumentiMusicali.Core.Properties.ImageIcons.Settings);
            rib5.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingFtpBackup));
            };
            var rib6 = panel1.Add("Scheduler", StrumentiMusicali.Core.Properties.ImageIcons.Settings);
            rib6.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.Scheduler));
            };

            AggiungiImpostazioniFatture(tabSetting);
            AggiungiImpostazioniArticoli(tabSetting);
            AggiungiImpostazioniProgramma(tabSetting);
        }

        private void AggiungiImpostazioniArticoli(RibbonMenuTab tabSetting)
        {
            var panel1 = tabSetting.Add("Impostazioni articoli");

            AggiungiPulsanteSetting(panel1, "Nomi listini prezzi", enAmbiente.NomeListiniClientiList);

            AggiungiPulsanteSetting(panel1, "Categorie", enAmbiente.CategorieArticoliList);
            AggiungiPulsanteSetting(panel1, "Periodi riordino", enAmbiente.RiordinoPeriodiList);

        }
        private void AggiungiImpostazioniFatture(RibbonMenuTab tabSetting)
        {
            var panel1 = tabSetting.Add("Impostazioni Fatture & Scontrino");

            panel1.Add("Tipi pagamenti fattura", StrumentiMusicali.Core.Properties.ImageIcons.Settings).Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.TipiPagamentiList));
            };

            panel1.Add("Tipi pagamenti scontrino", StrumentiMusicali.Core.Properties.ImageIcons.Settings)
                .Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.TipiPagamentiScontrinoList));
                };
            panel1.Add("Scontrino", StrumentiMusicali.Core.Properties.ImageIcons.Settings)
                .Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingScontrino));
                };
            panel1.Add("Intest. fattura per stampa", StrumentiMusicali.Core.Properties.ImageIcons.Settings)
                .Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingStampa));
                };

            panel1.Add("Mittente fattura", StrumentiMusicali.Core.Properties.ImageIcons.Settings)
                .Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingFatture));
                };

        }

        private void AggiungiImpostazioniProgramma(RibbonMenuTab tabSetting)
        {
            var panel1 = tabSetting.Add("Impostazioni programma");

            AggiungiPulsanteSetting(panel1, "Impostazioni programma", enAmbiente.SettingProgramma);



        }
        private void AggiungiPrincipale()
        {
            var tabImportExport = _menuTab.Add(@"Principale");
            if (LoginData.utenteLogin.Fatturazione)
            {
                var panel1 = tabImportExport.Add("Ambienti Fatturazione");
                panel1.Add("Fatturazione", StrumentiMusicali.Core.Properties.ImageIcons.Invoice).Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.FattureList));
                };
                panel1.Add(@"Clienti\Fornitori", StrumentiMusicali.Core.Properties.ImageIcons.Customer_48).Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ClientiList));
                };
            }



            var panel2 = tabImportExport.Add(@"Gestione anagrafica");
            panel2.Add("Gestione " + _settingProgramma.NomeAnagrafica, StrumentiMusicali.Core.Properties.ImageIcons.StrumentoMusicale).Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.StrumentiList));
            };
            if (_settingProgramma.GestioneLibri)
            {
                panel2.Add("Gestione libri", StrumentiMusicali.Core.Properties.ImageIcons.Libro_48).Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.LibriList));
                };
            }
            panel2.Add(@"Ricerca " + _settingProgramma.NomeAnagrafica, StrumentiMusicali.Core.Properties.ImageIcons.Search_48).Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.RicercaArticolo));
            };

            if (_settingProgramma.AmbientePagamentiRateali)
            {
                var panelPag = tabImportExport.Add("Pagamenti");

                panelPag.Add("Gestione Pagamenti", StrumentiMusicali.Core.Properties.ImageIcons.Payment)
                .Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.PagamentiList));
                };
            }
            if (LoginData.utenteLogin.AdminUtenti)
            {
                var panel3 = tabImportExport.Add("Utenti");
                var ribUtenti = panel3.Add("Utenti Login", StrumentiMusicali.Core.Properties.ImageIcons.Utenti);
                ribUtenti.Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.UtentiList));
                };
            }
        }

        private void AggiungiTabLog()
        {
            var tabImportExport = _menuTab.Add(@"Log");
            var panel1 = tabImportExport.Add("Principale");
            var ribSetting = panel1.Add("Visualizza log", StrumentiMusicali.Core.Properties.ImageIcons.LogView_48);
            ribSetting.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.LogViewList));
            };
        }
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            _logger.Error(e.Exception, "Application_ThreadException", null);
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
                    {
                        var contrArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.Ricerca);
                        var viewRicercaArt = new RicercaArticoliStyleView(contrArt);

                        this.ShowView(viewRicercaArt, obj.TipoEnviroment, contrArt);
                    }
                    break;
                case enAmbiente.ArticoliSottoscorta:
                    {
                        var contrArt = new ControllerListinoFornitori(null, true);
                        var viewRicercaArt =
                            new BaseGridViewGeneric<ListinoPrezziFornitoriItem,
                            ControllerListinoFornitori, ListinoPrezziFornitori>(contrArt, true);

                        this.ShowView(viewRicercaArt, obj.TipoEnviroment, contrArt);
                    }
                    break;

                case enAmbiente.CategorieArticoliList:
                    {
                        var contrArt = new ControllerCategorie();
                        var viewRicercaArt =
                            new BaseGridViewGeneric<CategoriaItem, ControllerCategorie,
                            Categoria>(contrArt);

                        this.ShowView(viewRicercaArt, obj.TipoEnviroment, contrArt);
                    }
                    break;
                case enAmbiente.NomeListiniClientiList:
                    {
                        var contrArt = new ControllerListinoPrezziNomi(false);
                        var viewRicercaArt =
                            new BaseGridViewGeneric<ListinoPrezziVenditaNomeItem, ControllerListinoPrezziNomi,
                            ListinoPrezziVenditaNome>(contrArt, false);

                        this.ShowView(viewRicercaArt, obj.TipoEnviroment, contrArt);
                    }
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
                    {
                        var controllerFatt = new ControllerFatturazione();

                        var viewFatt = new FattureListView(controllerFatt, enAmbiente.FattureList, enAmbiente.Fattura);

                        ShowView(viewFatt, obj.TipoEnviroment, controllerFatt);
                    }
                    break;
                case enAmbiente.TipiPagamentiList:
                    {
                        var controller = new ControllerTipiPagamentiFatture();

                        var view = new BaseGridViewGeneric<TipiPagamentoDocumentiItem, ControllerTipiPagamentiFatture, TipiPagamentoDocumenti>(controller, false);

                        ShowView(view, obj.TipoEnviroment, controller);
                    }
                    break;
                case enAmbiente.RiordinoPeriodiList:
                    {
                        var controller = new ControllerRiordinoPeriodi();

                        var view = new BaseGridViewGeneric<RiordinoPeriodiItem, ControllerRiordinoPeriodi, RiordinoPeriodi>(controller, false);

                        ShowView(view, obj.TipoEnviroment, controller);
                    }
                    break;
                case enAmbiente.TipiPagamentiScontrinoList:
                    {
                        var controller = new ControllerTipiPagamentiScontrino();

                        var view = new BaseGridViewGeneric<TipiPagamentoScontrinoItem, ControllerTipiPagamentiScontrino, TipiPagamentoScontrino>(controller, false);

                        ShowView(view, obj.TipoEnviroment, controller);
                    }
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

                    var viewCli = new SoggettiListView(controllerClienti);

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

                case enAmbiente.SettingScontrino:

                    ApriSettingScontrino();
                    break;
                case enAmbiente.SettingSito:

                    ApriSettingSito();
                    break;
                case enAmbiente.SettingProgramma:

                    ApriSettingProgramma();
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

        private void ApriSettingProgramma()
        {
            var setting = SettingProgrammaValidator.ReadSetting();

            var view = new GenericSettingView(setting);
            {
                view.OnSave += (a, b) =>
                {
                    using (var cur = new CursorManager())
                    {
                        view.Validate();
                        using (var save = new SaveEntityManager())
                        {
                            save.UnitOfWork.SettingProgrammaRepository.Update(setting);
                            save.SaveEntity(enSaveOperation.OpSave);
                        }
                    }
                };
                this.ShowView(view, enAmbiente.SettingProgramma);
            }

        }

        private void ApriSettingScheduler()
        {
            throw new NotImplementedException();
        }

        //private void ImportaCsvArticoli(ImportArticoliCSVMercatino obj)
        //{
        //    using (var controllerArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.Tutto))
        //    {
        //        controllerArt.ImportaCsvArticoli();
        //    }
        //}
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

        private void ImportaFatture(ImportaFattureAccess obj)
        {

            ControllerFatturazione.ImportaFatture();
        }

        private void ImportArticoliMulinoExcel(ImportArticoliMulino obj)
        {
            using (var import = new ImportCavalloPazzoExcel())
            {
                import.ImportFile();
            }
        }

        private void ImportMagExcel(ImportMagExcel obj)
        {
            using (var import = new ImportMagazziniExcel())
            {
                import.ImportFile();
            }
        }
    

        private void MainView_Disposed(object sender, EventArgs e)
        {
            if ((sender as UserControl).IsDisposed)
            {
                return;
            }
        }
    }
}