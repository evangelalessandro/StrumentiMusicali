using Autofac;
using Newtonsoft.Json;
using NLog;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Settings;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
	public class BaseController : IDisposable
	{
		internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		internal readonly string _PathSetting = Path.Combine(Application.StartupPath, @"settings.json");
		public const string TagCerca = "CERCA";

		public BaseController()
		{
		}

		public void ShowView(UserControl view, Settings.enAmbienti ambiente)
		{
			using (var frm = new Form())
			{
				ImpostaIconaETesto(ambiente, frm);

				if (view.MinimumSize.Height > 0)
				{
					frm.MinimumSize = new System.Drawing.Size(view.MinimumSize.Width, view.MinimumSize.Height + 190);
				}

				view.Dock = DockStyle.Fill;
				frm.Controls.Add(view);
				if (view is IMenu)
				{
					Ribbon ribbon1 = LoadMenu(view);
					InitRibbon(ribbon1);

					frm.Controls.Add(ribbon1);
				}
				if (view is GenericSettingView)
				{
					(view as GenericSettingView).OnClose += (
						b, c) => { frm.Close(); };
				}
				
				frm.Load += (a, b) =>
				{
					ReadSettingForm(ambiente, frm);
				};
				frm.ShowDialog();

				SavSettingForm(ambiente, frm);

				foreach (Control item in frm.Controls)
				{
					item.Dispose();
				}
			}
		}
		public string TestoAmbiente(enAmbienti ambiente)
		{
			switch (ambiente)
			{
				case enAmbienti.ClientiList:
					return"Clienti";
					
				case enAmbienti.Cliente:
					return"Cliente";
					
				case enAmbienti.Main:
					return"Principale";
					
				case enAmbienti.Fattura:
					return"Fattura";
					
				case enAmbienti.FattureList:
					return"Fatture";
					
				case enAmbienti.Articolo:
					return"Articolo";
					
				case enAmbienti.ArticoliList:
					return"Articoli";
					
				case enAmbienti.Magazzino:
					return"Magazzino";
					
				case enAmbienti.SettingFatture:
					return "Impostazioni fatture";
					
				case enAmbienti.SettingSito:
					return "Impostazioni sito";
					
				case enAmbienti.ScaricoMagazzino:
					return "Scarico Magazzino";
					
				case enAmbienti.LogView:
				 	return "Visualizzatore dei log";
					
				default:
					return "NIENTE DI IMPOSTATO";
					
			}
		}
		private void ImpostaIconaETesto(enAmbienti ambiente, Form frm)
		{
			switch (ambiente)
			{
				case enAmbienti.ClientiList:
					frm.Icon = GetIco(Properties.Resources.Customer_48);
					break;
				case enAmbienti.Cliente:
					frm.Icon = GetIco(Properties.Resources.Customer_48);
					break;
				case enAmbienti.Main:
					frm.Icon = GetIco(Properties.Resources.StrumentoMusicale);
					break;
				case enAmbienti.Fattura:
					frm.Icon = GetIco(Properties.Resources.Invoice);
					break;
				case enAmbienti.FattureList:
					frm.Icon = GetIco(Properties.Resources.Invoice);
					break;
				case enAmbienti.Articolo:
					frm.Icon = GetIco(Properties.Resources.StrumentoMusicale);
					break;
				case enAmbienti.ArticoliList:
					frm.Icon = GetIco(Properties.Resources.StrumentoMusicale);
					break;
				case enAmbienti.Magazzino:
					frm.Icon = GetIco(Properties.Resources.UnloadWareHouse);
					break;
				case enAmbienti.SettingFatture:
					frm.Icon = GetIco(Properties.Resources.Settings);
					break;
				case enAmbienti.SettingSito:
					frm.Icon = GetIco(Properties.Resources.Settings);
					break;
				case enAmbienti.ScaricoMagazzino:
					frm.Icon = GetIco(Properties.Resources.UnloadWareHouse);
					break;
				case enAmbienti.LogView:
					frm.Icon = GetIco(Properties.Resources.LogView_48);
					break;
				default:
					break;
			}
			frm.Text = TestoAmbiente(ambiente);
		}

		private Icon GetIco(Bitmap bitmap)
		{
			Bitmap bm = new Bitmap(bitmap);

			// Convert to an icon and use for the form's icon.
			return Icon.FromHandle(bm.GetHicon());
		}
		private static void InitRibbon(Ribbon ribbon1)
		{
			//
			// ribbon1
			//
			ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
			ribbon1.Location = new System.Drawing.Point(0, 0);
			ribbon1.Margin = new System.Windows.Forms.Padding(2);
			ribbon1.Minimized = false;
			ribbon1.Name = "ribbon1";
			//
			//
			//
			ribbon1.OrbDropDown.BorderRoundness = 8;
			ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
			ribbon1.OrbDropDown.Name = "";
			ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 447);
			ribbon1.OrbDropDown.TabIndex = 0;
			ribbon1.OrbDropDown.Visible = false;
			ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2010;
			//
			//
			//
			ribbon1.QuickAccessToolbar.Visible = false;
			ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
			ribbon1.Size = new System.Drawing.Size(851, 167);
			ribbon1.TabIndex = 0;

			ribbon1.TabsMargin = new System.Windows.Forms.Padding(6, 26, 20, 0);
			ribbon1.TabSpacing = 3;
			ribbon1.Text = "ribbon1";
			ribbon1.Dock = DockStyle.Top;
		}

		private static Ribbon LoadMenu(UserControl view)
		{
			var menu = ((IMenu)view).GetMenu();
			Ribbon ribbon1 = new Ribbon();

			(menu as INotifyPropertyChanged) .PropertyChanged += (a, e) =>
			{
				ribbon1.Enabled = menu.Enabled;
				ribbon1.Visible = menu.Visible;
			};

			foreach (var tab in menu.Tabs)
			{
				var rbTab = (new RibbonTab(tab.Testo));
				ribbon1.Tabs.Add(rbTab);
				(tab as INotifyPropertyChanged).PropertyChanged += (a, e) =>
				{
					rbTab.Enabled = tab.Enabled;
					rbTab.Visible = tab.Visible;
				};

				foreach (var pannello in tab.Pannelli)
				{
					var rbPannel = new RibbonPanel(pannello.Testo);
					rbTab.Panels.Add(rbPannel);
					(pannello as INotifyPropertyChanged).PropertyChanged += (a, e) =>
					{
						rbPannel.Enabled = pannello.Enabled;
						rbPannel.Visible = pannello.Visible;
					};
					foreach (var button in pannello.Pulsanti)
					{
						var rbButton = new RibbonButton(button.Testo);

						(button as INotifyPropertyChanged) .PropertyChanged += (a, e) =>
						{
							rbButton.Enabled = button.Enabled;
							rbButton.Visible = button.Visible;
							rbButton.Checked = button.Checked;
						};
						rbButton.LargeImage = button.Immagine;
						rbPannel.Items.Add(rbButton);
						rbButton.Click += (e, a) =>
						{
							button.PerformClick();
						};
					}
				}
			}

			return ribbon1;
		}

		private void SavSettingForm(enAmbienti ambiente, Form frm)
		{
			var dato = this.ReadSetting(ambiente);

			dato.Left = frm.Left;
			dato.Top = frm.Top;
			dato.StartPosition = frm.StartPosition;
			dato.FormMainWindowState = frm.WindowState;
			dato.SizeFormMain = frm.Size;

			this.SaveSetting(ambiente, dato);
		}

		private void ReadSettingForm(enAmbienti ambiente, Form frm)
		{
			try
			{
				var datiInit = this.ReadSetting(ambiente);

				frm.StartPosition = datiInit.StartPosition;
				frm.WindowState = datiInit.FormMainWindowState;
				frm.Size = datiInit.SizeFormMain;
				frm.Left = datiInit.Left;
				frm.Top = datiInit.Top;
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		public UserSettings ReadSetting()
		{
			UserSettings setting;
			if (File.Exists(_PathSetting))
			{
				var json = File.ReadAllText(_PathSetting);
				setting = JsonConvert.DeserializeObject<UserSettings>(json);
			}
			else
			{
				setting = new UserSettings();
			}
			return setting;
		}

		public FormRicerca ReadSetting(enAmbienti ambiente)
		{
			var setting = ReadSetting();
			if (setting.Form == null)
			{
				setting.Form = new System.Collections.Generic.List<Tuple<enAmbienti, FormRicerca>>();
			}
			var elem = setting.Form.Where(a => a.Item1 == ambiente).FirstOrDefault();
			if (elem == null)
			{
				setting.Form.Add(new Tuple<enAmbienti, FormRicerca>(ambiente, new FormRicerca()));
				SaveSetting(setting);
			}
			return setting.Form.Where(a => a.Item1 == ambiente).First().Item2;
		}

		public void SaveSetting(enAmbienti ambiente, FormRicerca formRicerca)
		{
			var setting = ReadSetting();
			setting.Form.RemoveAll(a => a.Item1 == ambiente);
			setting.Form.Add(new Tuple<enAmbienti, FormRicerca>(ambiente, formRicerca));
			SaveSetting(setting);
		}

		public void SaveSetting(UserSettings settings)
		{
			File.WriteAllText(_PathSetting,
				JsonConvert.SerializeObject(settings));
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~BaseController()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
			}
			// free native resources if there are any.
		}
	}
}