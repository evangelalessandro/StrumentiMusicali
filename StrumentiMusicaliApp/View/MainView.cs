using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using NLog;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Tab;
using StrumentiMusicali.App.Core.Exports;
using StrumentiMusicali.App.Core.Imports;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Generics;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App
{
    public partial class MainView : XtraUserControl, IMenu
    {
        internal readonly ILogger _logger = StrumentiMusicali.Core.Manager.ManagerLog.Logger;

        private BaseController _baseController;

        private MenuTab _menuTab = null;
        private DevExpress.XtraTab.XtraTabControl tab;

        public MainView(BaseController baseController)
        {
            _baseController = baseController;
            InitializeComponent();

            _logger.Debug("Form main init");

            tab = new DevExpress.XtraTab.XtraTabControl
            {
                Dock = DockStyle.Fill,
                ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders,
                HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.Always
            };
            tab.CloseButtonClick += Tab_CloseButtonClick;
            this.Controls.Add(tab);

            EventAggregator.Instance().Subscribe<GetNewTab>(TakeNewTab);
            EventAggregator.Instance().Subscribe<RemoveNewTab>(RemoveTab);


        }

        private void Tab_CloseButtonClick(object sender, System.EventArgs e)
        {
            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            var page = arg.Page as XtraTabPage;

            tab.TabPages.Remove(page);
            page.Dispose();
            GC.SuppressFinalize(page);
        }

        private void RemoveTab(RemoveNewTab obj)
        {
            tab.TabPages.Remove(obj.Tab);
        }

        private void TakeNewTab(GetNewTab obj)
        {
            obj.Tab = tab.TabPages.Add(obj.Text);

            obj.Tab.Tag = obj.Ambiente;
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

        private void AggiungiImpostazioniListiniClienti(RibbonMenuTab tabSetting)
        {
            var panel1 = tabSetting.Add("Impostazioni articoli");

            AggiungiPulsanteSetting(panel1, "Nomi listini prezzi", enAmbiente.NomeListiniClientiList);

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
            AggiungiImpostazioniListiniClienti(tabSetting);

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

        private void AggiungiComandiImportExport()
        {
            var tabImportExport = _menuTab.Add(@"Import\Export");
            var pnlExport = tabImportExport.Add("Export");
            var ribInvio = pnlExport.Add("Invio Articoli", StrumentiMusicali.Core.Properties.ImageIcons.Upload);
            ribInvio.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new InvioArticoliCSV());
            };
            var ribExport1 = pnlExport.Add("Export Tutti libri Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export);
            ribExport1.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.TuttoLibri });
            };
            var ribExport12 = pnlExport.Add("Export Tutti Strumenti Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export);
            ribExport12.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.TuttoStrumenti });
            };
            var pnlExport2 = tabImportExport.Add("Elenco Mancanti");
            var ribExport2 = pnlExport2.Add("Libri(Betta)", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export);
            ribExport2.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.SoloLibriMancanti });
            };
            var ribExport3 = pnlExport2.Add("X marca (Luca)", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export);
            ribExport3.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.PerMarca });
            };

            var pnlImport = tabImportExport.Add("Import");
            var ribImportCsv = pnlImport.Add("Import csv mercatino", StrumentiMusicali.Core.Properties.ImageIcons.ImportCsv);
            ribImportCsv.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ImportArticoliCSVMercatino());
            };
            var ribImport01 = pnlImport.Add("Import Tutti Libri Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export);
            ribImport01.Click += (s, e) =>
            {
                using (var mag = new ImportDaExportMagazzino(true))
                {
                    mag.ImportFile();
                }
            };
            var ribImport02 = pnlImport.Add("Import Tutti Strumenti Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.Excel_export);
            ribImport02.Click += (s, e) =>
            {
                using (var mag = new ImportDaExportMagazzino(false))
                {
                    mag.ImportFile();
                }
            };

            //var ribImportFatture = pnlImport.Add("Fatture Access", StrumentiMusicali.Core.Properties.ImageIcons.ImportInvoice);
            //ribImportFatture.Click += (s, e) =>
            //{
            //    EventAggregator.Instance().Publish(new ImportaFattureAccess());
            //};
            //var rib3 = pnlImport.Add("Import Mulino Excel", StrumentiMusicali.Core.Properties.ImageIcons.ImportCsv);
            //rib3.Click += (s, e) =>
            //{
            //    EventAggregator.Instance().Publish(new ImportArticoliMulino());
            //};
            //var rib4 = pnlImport.Add("Import Magazzini excel", StrumentiMusicali.Core.Properties.ImageIcons.ImportCsv);
            //rib4.Click += (s, e) =>
            //{
            //    EventAggregator.Instance().Publish(new ImportMagExcel());
            //};
        }

        private void AggiungiPrincipale()
        {
            var tabImportExport = _menuTab.Add(@"Principale");
            if (LoginData.utenteLogin.Fatturazione)
            {
                var panel1 = tabImportExport.Add("Ambienti Fatturazione");
                var ribFatt = panel1.Add("Fatturazione", StrumentiMusicali.Core.Properties.ImageIcons.Invoice);
                ribFatt.Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.FattureList));
                };
                var rib2 = panel1.Add(@"Clienti\Fornitori", StrumentiMusicali.Core.Properties.ImageIcons.Customer_48);
                rib2.Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ClientiList));
                };
            }
            //if (LoginData.utenteLogin.Magazzino)
            //{
            //	var ribMagaz = panel1.Add("Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.UnloadWareHouse);
            //	ribMagaz.Click += (s, e) =>
            //	{
            //		EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ScaricoMagazzino));
            //	};
            //}
            var panel2 = tabImportExport.Add(@"Gestione articoli\libri");
            var ribArticoli = panel2.Add("Gestione strumenti", StrumentiMusicali.Core.Properties.ImageIcons.StrumentoMusicale);
            ribArticoli.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.StrumentiList));
            };
            var ribLibri = panel2.Add("Gestione libri", StrumentiMusicali.Core.Properties.ImageIcons.Libro_48);
            ribLibri.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.LibriList));
            };
            var ribRicArticoli = panel2.Add(@"Ricerca articoli\libri", StrumentiMusicali.Core.Properties.ImageIcons.Search_48);
            ribRicArticoli.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.RicercaArticolo));
            };
            var panelPag = tabImportExport.Add("Pagamenti");

            var ribPag = panelPag.Add("Gestione Pagamenti", StrumentiMusicali.Core.Properties.ImageIcons.Payment);
            ribPag.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.PagamentiList));
            };

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
    }
}