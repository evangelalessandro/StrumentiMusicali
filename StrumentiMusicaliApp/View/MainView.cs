using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Events.Magazzino;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
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

		private RibbonMenuButton ribCercaArticolo;

		private RibbonMenuButton ribDeleteArt;

		private RibbonMenuButton ribDuplicaArt;

		private RibbonMenuButton ribEditArt;

		public MainView(BaseController baseController)
		{
			_baseController = baseController;
			InitializeComponent();
			dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;

			txtCerca.KeyUp += TxtCerca_KeyUp;
			EventAggregator.Instance().Subscribe<ArticoliToUpdate>(UpdateList);

			_logger.Debug("Form main init");
		}

		public List<ArticoloItem> GetDataAsync()
		{
			try
			{
				var datoRicerca = txtCerca.Text;
				List<ArticoloItem> list = new List<ArticoloItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.ArticoliRepository.Find(a => datoRicerca == "" || a.Titolo.Contains(datoRicerca)
						|| a.Testo.Contains(datoRicerca)
					).Select(a => new ArticoloItem
					{
						ID = a.ID,
						Titolo = a.Titolo,
						Entity = a,
						DataCreazione = a.DataCreazione,
						DataModifica = a.DataUltimaModifica,
						Pinned = a.Pinned
					}).ToList();
				}

				return list;
			}
			catch (Exception ex)
			{
				this.BeginInvoke(new Action(() =>
			   { ExceptionManager.ManageError(ex); }));
				return null;
			}
		}

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				AggiungiComandiArticolo();

				AggiungiComandiImportExport();

				AggiungiFatturazioneMagazzino();

				AggiungiImpostazioni();

				AggiungiTabLog();
			}
			return _menuTab;
		}

		private void AggiungiImpostazioni()
		{
			var tabImportExport = _menuTab.Add(@"Impostazioni");
			var panel1 = tabImportExport.Add("Principale");
			var ribSetting = panel1.Add("Dati fattura mittente", Properties.Resources.Settings);
			var ribSettingUrl = panel1.Add("Dati Sito & Upload", Properties.Resources.Settings);
			ribSetting.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enTipoEnviroment.SettingFatture));
			};
			ribSettingUrl.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enTipoEnviroment.SettingUrl));
			};
		}
		private void AggiungiTabLog()
		{
			var tabImportExport = _menuTab.Add(@"Log");
			var panel1 = tabImportExport.Add("Principale");
			var ribSetting = panel1.Add("Visualizza log", Properties.Resources.LogView_48);
			ribSetting.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enTipoEnviroment.LogView));
			};
			 
		}

		private void AggiungiComandiArticolo()
		{
			var tabArticoli = _menuTab.Add("Articoli");
			var panelComandiArticoli = tabArticoli.Add("Comandi Articoli");
			var ribCreArt = panelComandiArticoli.Add("Crea", Properties.Resources.Add);
			ribEditArt = panelComandiArticoli.Add(@"Vedi\Modifica", Properties.Resources.Edit);
			ribDeleteArt = panelComandiArticoli.Add("Cancella", Properties.Resources.Delete);
			ribDuplicaArt = panelComandiArticoli.Add("Duplica", Properties.Resources.Duplicate);
			var panelComandiArticoliCerca = tabArticoli.Add("Cerca");
			ribCercaArticolo = panelComandiArticoliCerca.Add("Cerca", Properties.Resources.Find);

			ribCreArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new ArticoloAdd());
			};
			ribDeleteArt.Click += (a, e) =>
			{
				using (var curs = new CursorManager())
				{
					EventAggregator.Instance().Publish<ArticoloDelete>(new ArticoloDelete(GetCurrentItemSelected()));
				}
			};
			ribEditArt.Click += (a, e) =>
			{
				EditArticolo();
			};
			ribDuplicaArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new ArticoloDuplicate());
			};
			ribCercaArticolo.Click += (a, e) =>
			{
				bool visibleCerca = pnlCerca.Visible;
				pnlCerca.Visible = !visibleCerca;
				splitter1.Visible = !visibleCerca;
				if (!visibleCerca)
				{
					pnlArticoli.Dock = DockStyle.Fill;
				}
				else
				{
					pnlCerca.Dock = DockStyle.Top;
				}
				UpdateButtonState();
			};
		}

		private void AggiungiComandiImportExport()
		{
			var tabImportExport = _menuTab.Add(@"Import\Export");
			var panel1 = tabImportExport.Add("Export");
			var ribInvio = panel1.Add("Invio Articoli", Properties.Resources.Upload);
			var panel2 = tabImportExport.Add("Import");
			var ribImportCsv = panel1.Add("Import csv mercatino", Properties.Resources.ImportCsv);
			var ribImportFatture = panel1.Add("Fatture Access", Properties.Resources.ImportInvoice);
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

		private void AggiungiFatturazioneMagazzino()
		{
			var tabImportExport = _menuTab.Add(@"Fatturazione Magazzino");
			var panel1 = tabImportExport.Add("Ambienti");
			var ribFatt = panel1.Add("Fatturazione", Properties.Resources.Invoice);
			ribFatt.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriAmbiente(enTipoEnviroment.Fatturazione));
			};

			var ribMagaz = panel1.Add("Magazzino", Properties.Resources.UnloadWareHouse);
			ribMagaz.Click += (s, e) =>
			{
				EventAggregator.Instance().Publish(new ApriGestioneMagazzino());
			};
		}

		private void dgvMaster_DoubleClick(object sender, EventArgs e)
		{
			var g = sender as DataGridView;
			if (g != null)
			{
				var p = g.PointToClient(MousePosition);
				var hti = g.HitTest(p.X, p.Y);
				if (hti.Type == DataGridViewHitTestType.ColumnHeader)
				{
					var columnIndex = hti.ColumnIndex;
					//You handled a double click on column header
					//Do what you need
				}
				else if (hti.Type == DataGridViewHitTestType.RowHeader || hti.Type == DataGridViewHitTestType.Cell)
				{
					var rowIndex = hti.RowIndex;
					//You handled a double click on row header
					EditArticolo();
				}
			}
		}

		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			UpdateButtonState();

			EventAggregator.Instance().Publish(new ArticoloSelected(GetCurrentItemSelected()));
		}

		private async void EditArticolo()
		{
			var item = _baseController.ReadSetting().settingSito;
			if (!item.CheckFolderImmagini())
			{
				return;
			}
			var itemSelected = (ArticoloItem)dgvMaster.SelectedRows[0].DataBoundItem;
			using (var frm = new Forms.DettaglioArticoloView(itemSelected, item))
			{
				_baseController.ShowView(frm, Settings.enAmbienti.Articolo);
			}

			await RefreshData();
			await dgvMaster.SelezionaRiga(itemSelected.ID);
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			var dato = _baseController.ReadSetting(Settings.enAmbienti.Main);
			txtCerca.Text = dato.LastStringaRicerca;

			UpdateButtonState();
			await RefreshData();

			await dgvMaster.SelezionaRiga(dato.LastItemSelected);

			UpdateButtonState();

			_logger.Debug("Form load");
		}

		private ArticoloItem GetCurrentItemSelected()
		{
			ArticoloItem item = null;
			if (dgvMaster.SelectedRows.Count > 0)
			{
				item = (ArticoloItem)dgvMaster.SelectedRows[0].DataBoundItem;
			}
			return item;
		}

		private async Task RefreshData()
		{
			var data = await Task.Run(() => { return GetDataAsync(); });
			if (data == null)
				return;
			dgvMaster.DataSource = data;
			dgvMaster.Columns["Pinned"].Visible = false;
			dgvMaster.Columns["Entity"].Visible = false;
		}

		private async void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				var dato = _baseController.ReadSetting(Settings.enAmbienti.Main);
				dato.LastStringaRicerca = txtCerca.Text;
				_baseController.SaveSetting(Settings.enAmbienti.Main, dato);
				await RefreshData();
			}
		}

		private void UpdateButtonState()
		{
			_menuTab.Enabled = !(dgvMaster.DataSource == null);

			ribEditArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribDuplicaArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribDeleteArt.Enabled = dgvMaster.SelectedRows.Count > 0;
			ribCercaArticolo.Checked = pnlCerca.Visible;
		}

		private async void UpdateList(ArticoliToUpdate obj)
		{
			await RefreshData();
		}
	}
}