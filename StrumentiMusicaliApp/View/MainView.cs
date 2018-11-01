using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App
{
	public partial class MainView : UserControl, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		private BaseController _baseController;

		private MenuTab _menuTab = null;

		 
		public MainView(BaseController baseController)
		{
			_baseController = baseController;
			InitializeComponent();
		 
			_logger.Debug("Form main init");
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
			var rib1 = panel1.Add("Dati fattura mittente", Properties.Resources.Settings);
			rib1.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.SettingFatture));
			};
			var rib2 = panel1.Add("Dati Sito & Upload", Properties.Resources.Settings);
			rib2.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.SettingSito));
			};

			var rib3 = panel1.Add("Intestazione stampa fattura", Properties.Resources.Settings);
			rib3.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.SettingStampa));
			};

		}
		private void AggiungiTabLog()
		{
			var tabImportExport = _menuTab.Add(@"Log");
			var panel1 = tabImportExport.Add("Principale");
			var ribSetting = panel1.Add("Visualizza log", Properties.Resources.LogView_48);
			ribSetting.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.LogView));
			};
			 
		}

		 
		private void AggiungiComandiImportExport()
		{
			var tabImportExport = _menuTab.Add(@"Import\Export");
			var pnlExport = tabImportExport.Add("Export");
			var ribInvio = pnlExport.Add("Invio Articoli", Properties.Resources.Upload);

			var pnlImport = tabImportExport.Add("Import");
			var ribImportCsv = pnlImport.Add("Import csv mercatino", Properties.Resources.ImportCsv);
			var ribImportFatture = pnlImport.Add("Fatture Access", Properties.Resources.ImportInvoice);
			ribImportFatture.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportaFattureAccess());
			};
			ribImportCsv.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ImportArticoliCSVMercatino());
			};
			ribInvio.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new InvioArticoliCSV());
			};
		}

		private void AggiungiPrincipale()
		{
			var tabImportExport = _menuTab.Add(@"Principale");
			var panel1 = tabImportExport.Add("Ambienti");
			var ribFatt = panel1.Add("Fatturazione", Properties.Resources.Invoice);
			ribFatt.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.FattureList));
			};
			var rib2 = panel1.Add("Clienti", Properties.Resources.Customer_48);
			rib2.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.ClientiList));
			};
			var ribMagaz = panel1.Add("Magazzino", Properties.Resources.UnloadWareHouse);
			ribMagaz.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.ScaricoMagazzino));
			};

			var ribArticoli = panel1.Add("Articoli", Properties.Resources.StrumentoMusicale);
			ribArticoli.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enAmbienti.ArticoliList));
			};
		}
		  
	     
	}
}