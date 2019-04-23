﻿using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using NLog;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.Exports;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Events.Tab;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Enums;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App
{
	public partial class MainView : UserControl, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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
			var ribExport1 = pnlExport.Add("Export Stato Magazzino", Properties.Resources.Add);
			ribExport1.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ExportMagazzino());
			};

			var ribExport2 = pnlExport.Add("Elenco libri mancanti", Properties.Resources.Add);
			ribExport2.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ExportMagazzino() { SoloLibriMancanti = true });
			};


			var pnlImport = tabImportExport.Add("Import");
			var ribImportCsv = pnlImport.Add("Import csv mercatino", Properties.Resources.ImportCsv);
			ribImportCsv.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportArticoliCSVMercatino());
			};

			var ribImportFatture = pnlImport.Add("Fatture Access", Properties.Resources.ImportInvoice);
			ribImportFatture.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportaFattureAccess());
			};
			var rib3 = pnlImport.Add("Import Mulino Excel", Properties.Resources.ImportCsv);
			rib3.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportArticoliMulino());
			};
			var rib4 = pnlImport.Add("Import Magazzini excel", Properties.Resources.ImportCsv);
			rib4.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportMagExcel());
			};
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
			var panel2 = tabImportExport.Add("Gestione articoli");
			var ribArticoli = panel2.Add("Gestione articoli", Properties.Resources.StrumentoMusicale);
			ribArticoli.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ArticoliList));
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