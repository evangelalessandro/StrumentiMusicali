using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using NLog;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Tab;
using StrumentiMusicali.App.Core.Exports;
using StrumentiMusicali.App.Core.Imports;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Fatture;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App
{
    public partial class MainView : UserControl, IMenu
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

                AggiungiTabLog();

                AggiungiComandiImportExport();
            }
            return _menuTab;
        }

        private void AggiungiImpostazioni()
        {
            var tabImportExport = _menuTab.Add(@"Impostazioni");
            var panel1 = tabImportExport.Add("Principale");
            var rib1 = panel1.Add("Mittente fattura", Properties.Resources.Settings);
            rib1.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingFatture));
            };
            var rib2 = panel1.Add("Sito & Upload", Properties.Resources.Settings);
            rib2.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingSito));
            };

            var rib3 = panel1.Add("Intest. fattura per stampa", Properties.Resources.Settings);
            rib3.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingStampa));
            };

            var rib4 = panel1.Add("Depositi", Properties.Resources.Settings);
            rib4.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.DepositoList));
            };

            var rib5 = panel1.Add("Backup ftp", Properties.Resources.Settings);
            rib5.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingFtpBackup));
            };
            var rib6 = panel1.Add("Scheduler", Properties.Resources.Settings);
            rib6.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.Scheduler));
            };
            var rib7 = panel1.Add("Scontrino", Properties.Resources.Settings);
            rib7.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.SettingScontrino));
            };
        }

        private void AggiungiTabLog()
        {
            var tabImportExport = _menuTab.Add(@"Log");
            var panel1 = tabImportExport.Add("Principale");
            var ribSetting = panel1.Add("Visualizza log", Properties.Resources.LogView_48);
            ribSetting.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.LogViewList));
            };
        }

        private void AggiungiComandiImportExport()
        {
            var tabImportExport = _menuTab.Add(@"Import\Export");
            var pnlExport = tabImportExport.Add("Export");
            var ribInvio = pnlExport.Add("Invio Articoli", Properties.Resources.Upload);
            ribInvio.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new InvioArticoliCSV());
            };
            var ribExport1 = pnlExport.Add("Export Tutti libri Magazzino", Properties.Resources.Excel_export);
            ribExport1.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.TuttoLibri });
            };
            var ribExport12 = pnlExport.Add("Export Tutti Strumenti Magazzino", Properties.Resources.Excel_export);
            ribExport12.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.TuttoStrumenti});
            };
            var pnlExport2 = tabImportExport.Add("Elenco Mancanti");
            var ribExport2 = pnlExport2.Add("Libri(Betta)", Properties.Resources.Excel_export);
            ribExport2.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.SoloLibriMancanti });
            };
            var ribExport3 = pnlExport2.Add("X marca (Luca)", Properties.Resources.Excel_export);
            ribExport3.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ExportMagazzino() { TipoExp = ExportMagazzino.TipoExport.PerMarca });
            };

            var pnlImport = tabImportExport.Add("Import");
            var ribImportCsv = pnlImport.Add("Import csv mercatino", Properties.Resources.ImportCsv);
            ribImportCsv.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ImportArticoliCSVMercatino());
            };
            var ribImport01 = pnlImport.Add("Import Tutti Libri Magazzino", Properties.Resources.Excel_export);
            ribImport01.Click += (s, e) =>
            {
                using (var mag = new ImportDaExportMagazzino(true))
                {
                    mag.ImportFile();
                }
            };
            var ribImport02 = pnlImport.Add("Import Tutti Strumenti Magazzino", Properties.Resources.Excel_export);
            ribImport02.Click += (s, e) =>
            {
                using (var mag = new ImportDaExportMagazzino(false))
                {
                    mag.ImportFile();
                }
            };

            //var ribImportFatture = pnlImport.Add("Fatture Access", Properties.Resources.ImportInvoice);
            //ribImportFatture.Click += (s, e) =>
            //{
            //    EventAggregator.Instance().Publish(new ImportaFattureAccess());
            //};
            //var rib3 = pnlImport.Add("Import Mulino Excel", Properties.Resources.ImportCsv);
            //rib3.Click += (s, e) =>
            //{
            //    EventAggregator.Instance().Publish(new ImportArticoliMulino());
            //};
            //var rib4 = pnlImport.Add("Import Magazzini excel", Properties.Resources.ImportCsv);
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
                var ribFatt = panel1.Add("Fatturazione", Properties.Resources.Invoice);
                ribFatt.Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.FattureList));
                };
                var rib2 = panel1.Add("Clienti", Properties.Resources.Customer_48);
                rib2.Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ClientiList));
                };
            }
            //if (LoginData.utenteLogin.Magazzino)
            //{
            //	var ribMagaz = panel1.Add("Magazzino", Properties.Resources.UnloadWareHouse);
            //	ribMagaz.Click += (s, e) =>
            //	{
            //		EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ScaricoMagazzino));
            //	};
            //}
            var panel2 = tabImportExport.Add(@"Gestione articoli\libri");
            var ribArticoli = panel2.Add("Gestione strumenti", Properties.Resources.StrumentoMusicale);
            ribArticoli.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.StrumentiList));
            };
            var ribLibri = panel2.Add("Gestione libri", Properties.Resources.Libro_48);
            ribLibri.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.LibriList));
            };
            var ribRicArticoli = panel2.Add(@"Ricerca articoli\libri", Properties.Resources.Search_48);
            ribRicArticoli.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.RicercaArticolo));
            };
            var panelPag = tabImportExport.Add("Pagamenti");

            var ribPag = panelPag.Add("Gestione Pagamenti", Properties.Resources.Payment);
            ribPag.Click += (s, e) =>
            {
                EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.PagamentiList));
            };

            if (LoginData.utenteLogin.AdminUtenti)
            {
                var panel3 = tabImportExport.Add("Utenti");
                var ribUtenti = panel3.Add("Utenti Login", Properties.Resources.Utenti);
                ribUtenti.Click += (s, e) =>
                {
                    EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.UtentiList));
                };
            }
        }
    }
}