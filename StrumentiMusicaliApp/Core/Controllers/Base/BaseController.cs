using Autofac;
using Newtonsoft.Json;
using NLog;
using StrumentiMusicali.App.Core.Events.Tab;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Attributes;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
    public class BaseController : IDisposable, IKeyController
    {
        internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        internal readonly string _PathSetting = Path.Combine(Application.StartupPath, @"settings.json");
        public readonly Guid _INSTANCE_KEY = Guid.NewGuid();

        Guid IKeyController.INSTANCE_KEY { get { return _INSTANCE_KEY; } }

        static Subscription<ForceCloseActiveFormView> _sub1 = EventAggregator.Instance().Subscribe<ForceCloseActiveFormView>(a =>
        {

            var frm = _PreviusForm.Last();
            if (frm != null)
            {
                frm.Close();
            }
        });
        public BaseController()
        {
            //


        }
        public const bool ModalitàAForm = false;
        public static T GetAttribute<T>(Enum enumValue) where T : Attribute
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            return (T)member?.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
        public void ShowView(UserControl view, enAmbiente ambiente, BaseController controller = null, bool disposeForm = true, bool forceFormView = false)
        {
            var attr = GetAttribute<UIAmbienteAttribute>(ambiente);

            if (ModalitàAForm || (attr != null && attr.OnlyViewInForm) || forceFormView)
            {
                AggiungiInForm(view, ambiente, controller, disposeForm);
            }
            else
            {
                using (var curs = new Manager.CursorManager())
                {
                    AggiungiTab(view, ambiente, controller, disposeForm);
                }
            }
        }

        public static List<(enAmbiente Ambiente, DevExpress.XtraTab.XtraTabPage Pagina, MenuTab MenuPagina)> AmbientiAttivi { get; set; }
            = new List<(enAmbiente Ambiente, DevExpress.XtraTab.XtraTabPage Pagina, MenuTab MenuPagina)>();



        private void AggiungiTab(UserControl view, enAmbiente ambiente, BaseController controller, bool disposeForm)
        {
            var attr = GetAttribute<UIAmbienteAttribute>(ambiente);

            var currentAlreadyPresentItem = AmbientiAttivi.Where(a => a.Ambiente == ambiente).DefaultIfEmpty(
                (enAmbiente.NonSpecificato, null, null)).FirstOrDefault();


            if (currentAlreadyPresentItem.Ambiente == ambiente && attr != null && attr.Exclusive)
            {
                ((DevExpress.XtraTab.XtraTabControl)currentAlreadyPresentItem.Pagina.Parent).SelectedTabPage
                    = currentAlreadyPresentItem.Pagina;
                currentAlreadyPresentItem.MenuPagina.Tabs[0].PerformSelect();

                view.Dispose();
                controller.Dispose();

                return;
            }
            if (attr.AmbienteMutuale != enAmbiente.NonSpecificato)
            {
                var item = AmbientiAttivi.Where(a => a.Ambiente
                 == attr.AmbienteMutuale).DefaultIfEmpty(
                     (enAmbiente.NonSpecificato, null, null)).FirstOrDefault().Ambiente;
                if (item != enAmbiente.NonSpecificato)
                {
                    MessageBox.Show("Occorre chiudere l'ambiente '" + TestoAmbiente(item) + "'");
                    view.Dispose();
                    controller.Dispose();
                    return;
                }
            }
            bool closed = false;
            MenuTab menu = null;
            if (controller != null && controller is IMenu)
            {
                menu = (controller as IMenu).GetMenu();
            }
            else if (view is IMenu)
            {
                menu = (view as IMenu).GetMenu();
            }
            if (menu != null)
            {
                menu.Tabs[0].Testo = TestoAmbiente(ambiente);

                LoadMenu(menu, _ribbonMaster);
            }
            ICloseSave closeSave = null;
            if (controller != null && controller is ICloseSave)
            {
                closeSave = (controller as ICloseSave);
            }
            else if (view is ICloseSave)
            {
                closeSave = (view as ICloseSave);
            }

            var arg = new GetNewTab(TestoAmbiente(ambiente), ambiente, closeSave);
            EventAggregator.Instance().Publish(arg);

            var newTab = arg.Tab;

            var tabControl = ((DevExpress.XtraTab.XtraTabControl)newTab.Parent);

            ((DevExpress.XtraTab.XtraTabControl)newTab.Parent).ControlRemoved += (b, c) =>
            {
                if (c.Control == newTab)
                {
                    if (closeSave != null)
                        closeSave.RaiseClose();
                }
            };
            AmbientiAttivi.Add((ambiente, newTab, menu));

            tabControl.SelectedPageChanged += (b, c) =>
              {

                  var visible = (tabControl.SelectedTabPage == newTab);

                  foreach (var item in menu.Tabs)
                  {
                      item.Visible = visible;
                      if (visible)
                      {
                          item.PerformSelect();
                      }

                  }
                  //if (actionSelectedIndex != null)
                  //{
                  // actionSelectedIndex.Invoke();
                  //}

              };

            view.Dock = DockStyle.Fill;
            newTab.Controls.Add(view);

            tabControl.SelectedTabPage = newTab;

            //view.BackColor = Color.Transparent;
            //view.ForeColor= Color.White;

            if (closeSave != null)
            {
                (closeSave as ICloseSave).OnClose += (
                    b, c) =>
                {
                    EventAggregator.Instance().Publish(new RemoveNewTab(newTab));
                    if (closed)
                        return;
                    closed = true;
                    if (!disposeForm)
                    {
                        newTab.Controls.Remove(view);
                    }
                    AmbientiAttivi.RemoveAll(a => a.Ambiente == ambiente);

                    RemoveMenu(menu, _ribbonMaster);

                    view.Dispose();
                    if (controller != null)
                    {
                        controller.Dispose();
                        GC.SuppressFinalize(controller);
                    }
                    GC.SuppressFinalize(view);
                    view = null;
                    if (!disposeForm)
                    {
                        newTab.Dispose();
                    }
                };
            }
            if (menu != null)
            {
                //seleziono la tab
                menu.Tabs[0].PerformSelect();
            }
            //while (closed == false)
            //{
            //	Application.DoEvents();
            //	if (_closeMain)
            //		break;
            //}


            //ribbonMaster.ActiveTab = ribbonMaster.PreviousTab;
        }

        private void RemoveMenu(MenuTab menu, Ribbon ribbon)
        {


            foreach (var tab in menu.Tabs)
            {
                var rbTab = ribbon.Tabs.Where(a => a.Tag.ToString() == tab.GetHashCode().ToString()).FirstOrDefault();

                if (rbTab == null)
                    break;

                foreach (var pannello in tab.Pannelli)
                {
                    var rbPannel = rbTab.Panels.Where(a => a.Tag.ToString() == pannello.GetHashCode().ToString()).First();

                    foreach (var button in pannello.Pulsanti)
                    {
                        var rbButton = rbPannel.Items.Where(a => a.Tag.ToString() == button.GetHashCode().ToString()).First();
                        rbPannel.Items.Remove(rbButton);

                    }
                    rbTab.Panels.Remove(rbPannel);

                }
                ribbon.Tabs.Remove(rbTab);
            }

        }

        /// <summary>
        /// ribbon della form main
        /// </summary>
        private static Ribbon _ribbonMaster = null;
        private static Form _MainForm;
        private static List<Form> _PreviusForm = new List<Form>();
        private void AggiungiInForm(UserControl view, enAmbiente ambiente, BaseController controller, bool disposeForm)
        {
            Ribbon ribbon1 = null;

            try
            {
                using (var frm = new Form())
                {
                    Action action = new Action(() =>
                    {
                        ReadSettingForm(ambiente, frm);
                    });

                    if (action != null)
                    {
                        action.Invoke();
                        action = null;
                    }
                    //frm.Activated += (a, b) =>
                    //{

                    //};

                    ImpostaIconaETesto(ambiente, frm);

                    if (view.MinimumSize.Height > 0)
                    {
                        frm.MinimumSize = new System.Drawing.Size(view.MinimumSize.Width, view.MinimumSize.Height + 190);
                    }

                    view.Dock = DockStyle.Fill;
                    frm.Controls.Add(view);
                    MenuTab menu = null;
                    if (controller != null && controller is IMenu)
                    {
                        menu = (controller as IMenu).GetMenu();
                    }
                    else if (view is IMenu)
                    {
                        menu = (view as IMenu).GetMenu();
                    }
                    if (menu != null)
                    {
                        ribbon1 = LoadMenu(menu);
                        InitRibbon(ribbon1);

                        frm.Controls.Add(ribbon1);
                    }

                    if (ambiente == enAmbiente.Main)
                    {
                        AddStatusBarProgress(frm);
                        ProgressManager.Instance().RaiseProChange();
                    }
                    if (controller != null && controller is ICloseSave)
                    {
                        (controller as ICloseSave).OnClose += (
                            b, c) =>
                        { frm.Close(); };
                    }
                    else if (view is ICloseSave)
                    {
                        (view as ICloseSave).OnClose += (
                            b, c) =>
                        { frm.Close(); };
                    }

                    frm.IsMdiContainer = true;
                    if (ambiente == enAmbiente.Main)
                    {
                        _ribbonMaster = ribbon1;
                        _MainForm = frm;
                        _PreviusForm.Add(frm);
                        frm.FormClosing += Frm_FormClosing;

                        frm.ShowDialog();

                    }
                    else
                    {
                        var last = _PreviusForm.Last();
                        _PreviusForm.Add(frm);

                        frm.ShowDialog(last);
                        _PreviusForm.Remove(frm);
                    }


                    SavSettingForm(ambiente, frm);

                    //if (ribbon1!=null)
                    //{
                    //	foreach (var itemTab in ribbon1.Tabs.ToList())
                    //	{
                    //		foreach (var itemPnl in itemTab.Panels.ToList())
                    //		{
                    //			foreach (var itemButt in itemPnl.Items.ToList())
                    //			{
                    //				itemPnl.Items.Remove(itemButt);
                    //				itemButt.Dispose();
                    //			}
                    //			itemTab.Panels.Remove(itemPnl);
                    //			itemPnl.Dispose();
                    //		}
                    //		ribbon1.Tabs.Remove(itemTab);
                    //		itemTab.Dispose();
                    //	}
                    //	ribbon1.Dispose();

                    //}
                    if (!disposeForm && frm != null)
                    {
                        frm.Controls.Remove(view);
                        view.Dispose();
                        GC.SuppressFinalize(view);
                    }
                }
            }
            finally
            {

            }
        }

        private void Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!MessageManager.QuestionMessage("Sei sicuro di volere chiudere l'applicazione?"))
            {
                e.Cancel = true;
                return;
            }
            e.Cancel = false;


        }

        private void AddStatusBarProgress(Form form)
        {
            // StatusBar
            // 
            ToolStripStatusLabel StatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            StatusBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            StatusBar.ForeColor = System.Drawing.Color.Blue;
            StatusBar.LinkColor = System.Drawing.Color.Navy;
            StatusBar.Name = "StatusBar";
            StatusBar.Size = new System.Drawing.Size(732, 20);
            StatusBar.Spring = true;
            StatusBar.Text = "Status Messages Go Here";
            StatusBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProgressBar
            // 
            ToolStripProgressBar ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            ProgressBar.ForeColor = System.Drawing.Color.Yellow;
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new System.Drawing.Size(400, 19);

            // 
            // StatusStrip
            // 
            ToolStrip StatusStrip = new System.Windows.Forms.ToolStrip();
            StatusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
             StatusBar, ProgressBar});
            StatusStrip.Location = new System.Drawing.Point(0, 0);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(899, 25);
            StatusStrip.TabIndex = 0;

            (ProgressManager.Instance() as INotifyPropertyChanged).PropertyChanged += (a, b) =>
            {

                StatusStrip.Visible = ProgressManager.Instance().Visible;
                ProgressBar.Maximum = ProgressManager.Instance().Max;
                ProgressBar.Value = ProgressManager.Instance().Value;
                StatusBar.Text = ProgressManager.Instance().Messaggio;
                Application.DoEvents();

            };

            form.Controls.Add(StatusStrip);
        }

        public string TestoAmbiente(enAmbiente ambiente)
        {
            switch (ambiente)
            {
                case enAmbiente.ClientiList:
                    return "Clienti";

                case enAmbiente.Cliente:
                    return "Cliente";

                case enAmbiente.Main:
                    return "Gestione magazzino e fatturazione";

                case enAmbiente.Fattura:
                    return "Fattura";

                case enAmbiente.FattureList:
                    return "Fatture";

                case enAmbiente.StrumentiDetail:
                    return "Strumento";

                case enAmbiente.StrumentiList:
                    return "Gestione Strumenti";

                case enAmbiente.LibriDetail:
                    return "Libro";

                case enAmbiente.LibriList:
                    return "Gestione Libri";

                case enAmbiente.Magazzino:
                    return "Magazzino";

                case enAmbiente.SettingFatture:
                    return "Impostazioni fatture";

                case enAmbiente.SettingSito:
                    return "Impostazioni sito";

                case enAmbiente.SettingDocPagamenti:
                    return "Impostazioni documenti pagamenti";

                case enAmbiente.SettingFtpBackup:
                    return "Impostazioni ftp backup";

                case enAmbiente.ScaricoMagazzino:
                    return "Scarico Magazzino";

                case enAmbiente.LogViewList:
                    return "Visualizzatore dei log";

                case enAmbiente.SettingStampa:
                    return "Settaggi di stampa fattura";

                case enAmbiente.FattureRigheList:
                    break;
                case enAmbiente.FattureRigheDett:
                    return "Dettaglio riga";
                case enAmbiente.Deposito:
                    return "Deposito";
                case enAmbiente.DepositoList:
                    return "Depositi";
                case enAmbiente.ArticoloSconto:
                    return "Sconta articoli";
                case enAmbiente.UtentiList:
                    return "Utenti";
                case enAmbiente.Utente:
                    return "Utente";
                case enAmbiente.PagamentiList:
                    return "Pagamenti";
                case enAmbiente.Pagamento:
                    return "Pagamento";
                case enAmbiente.RicercaArticolo:
                    return "Ricerca articolo";

                default:
                    return "NIENTE DI IMPOSTATO";

            }
            return "NIENTE DI IMPOSTATO";

        }
        private void ImpostaIconaETesto(enAmbiente ambiente, Form frm)
        {
            switch (ambiente)
            {
                case enAmbiente.PagamentiList:
                case enAmbiente.Pagamento:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Payment);
                    break;
                case enAmbiente.ClientiList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Customer_48);
                    break;
                case enAmbiente.UtentiList:
                case enAmbiente.Utente:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Utenti);
                    break;
                case enAmbiente.Cliente:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Customer_48);
                    break;
                case enAmbiente.Main:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.StrumentoMusicale);
                    break;
                case enAmbiente.RicercaArticolo:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Search_48);
                    break;
                case enAmbiente.Fattura:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Invoice);
                    break;
                case enAmbiente.FattureList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Invoice);
                    break;
                case enAmbiente.StrumentiDetail:
                case enAmbiente.StrumentiList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.StrumentoMusicale);
                    break;
                case enAmbiente.LibriDetail:
                case enAmbiente.LibriList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Libro_48);
                    break;
                case enAmbiente.Magazzino:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.UnloadWareHouse);
                    break;
                case enAmbiente.SettingFatture:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Settings);
                    break;
                case enAmbiente.SettingFtpBackup:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Settings);
                    break;
                case enAmbiente.SettingDocPagamenti:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Settings);
                    break;
                case enAmbiente.SettingSito:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Settings);
                    break;
                case enAmbiente.ScaricoMagazzino:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.UnloadWareHouse);
                    break;
                case enAmbiente.LogViewList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.LogView_48);
                    break;
                case enAmbiente.SettingStampa:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Settings);
                    break;
                case enAmbiente.FattureRigheList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Invoice);
                    break;
                case enAmbiente.FattureRigheDett:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Invoice);
                    break;
                case enAmbiente.Deposito:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Depositi);
                    break;
                case enAmbiente.DepositoList:
                    frm.Icon = UtilityView.GetIco(Properties.Resources.Depositi);
                    break;
                default:
                    break;
            }
            frm.Text = TestoAmbiente(ambiente);
        }


        private void InitRibbon(Ribbon ribbon1)
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
            ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2007;
            //
            //
            //
            ribbon1.QuickAccessToolbar.Visible = false;
            ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            ribbon1.Size = new System.Drawing.Size(851, 180);
            ribbon1.TabIndex = 0;

            ribbon1.TabsMargin = new System.Windows.Forms.Padding(6, 26, 20, 0);
            ribbon1.TabSpacing = 3;
            ribbon1.Text = "ribbon1";
            ribbon1.Dock = DockStyle.Top;
        }

        private static Ribbon LoadMenu(MenuTab menu, Ribbon ribbon = null)
        {
            Ribbon ribbon1 = ribbon;

            if (ribbon1 == null)
            {
                ribbon1 = new Ribbon();
            }

            (menu as INotifyPropertyChanged).PropertyChanged += (a, e) =>
           {
               ribbon1.Enabled = menu.Enabled;
               ribbon1.Visible = menu.Visible;
           };

            foreach (var tab in menu.Tabs)
            {
                var rbTab = (new RibbonTab(tab.Testo));
                rbTab.Tag = tab.GetHashCode();
                ribbon1.Tabs.Add(rbTab);
                tab.OnSelected += (b, c) =>
                {
                    ribbon1.ActiveTab = rbTab;
                };

                (tab as INotifyPropertyChanged).PropertyChanged += (a, e) =>
                {
                    rbTab.Enabled = tab.Enabled;
                    rbTab.Visible = tab.Visible;
                };

                foreach (var pannello in tab.Pannelli)
                {
                    var rbPannel = new RibbonPanel(pannello.Testo);
                    rbPannel.Tag = pannello.GetHashCode();
                    rbTab.Panels.Add(rbPannel);
                    (pannello as INotifyPropertyChanged).PropertyChanged += (a, e) =>
                    {
                        rbPannel.Enabled = pannello.Enabled;
                        rbPannel.Visible = pannello.Visible;
                    };
                    foreach (var button in pannello.Pulsanti)
                    {
                        var rbButton = new RibbonButton(button.Testo);


                        rbButton.Tag = button.GetHashCode();
                        (button as INotifyPropertyChanged).PropertyChanged += (a, e) =>
                       {
                           UpdateButton(button, rbButton);
                       };
                        UpdateButton(button, rbButton);

                        rbButton.LargeImage = button.Immagine.Clone() as Bitmap;
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

        private static void UpdateButton(MenuRibbon.RibbonMenuButton button, RibbonButton rbButton)
        {
            rbButton.Enabled = button.Enabled;
            rbButton.Visible = button.Visible;
            rbButton.Checked = button.Checked;
        }

        private void SavSettingForm(enAmbiente ambiente, Form frm)
        {
            var dato = this.ReadSetting(ambiente);

            dato.Left = frm.Left;
            dato.Top = frm.Top;
            dato.StartPosition = frm.StartPosition;
            dato.FormMainWindowState = frm.WindowState;
            dato.SizeFormMain = frm.Size;

            this.SaveSetting(ambiente, dato);
        }

        private void ReadSettingForm(enAmbiente ambiente, Form frm)
        {
            try
            {
                var datiInit = this.ReadSetting(ambiente);
                frm.SuspendLayout();
                frm.StartPosition = datiInit.StartPosition;
                frm.WindowState = datiInit.FormMainWindowState;
                frm.Size = datiInit.SizeFormMain;
                frm.Left = datiInit.Left;
                frm.Top = datiInit.Top;
                frm.ResumeLayout();
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

        public FormRicerca ReadSetting(enAmbiente ambiente)
        {
            var setting = ReadSetting();
            if (setting.Form == null)
            {
                setting.Form = new List<(enAmbiente ambiente, FormRicerca form)>();
            }
            if (setting.Form.Where(a => a.ambiente == ambiente).Count() == 0)
            {
                setting.Form.Add((ambiente, new FormRicerca()));
                SaveSetting(setting);
            }
            return setting.Form.Where(a => a.Item1 == ambiente).First().Item2;
        }

        public void SaveSetting(enAmbiente ambiente, FormRicerca formRicerca)
        {
            if (ambiente == enAmbiente.Main)
            {
                return;
            }

            var setting = ReadSetting();
            setting.Form.RemoveAll(a => a.Item1 == ambiente);
            setting.Form.Add((ambiente, formRicerca));
            SaveSetting(setting);
        }

        public void SaveSetting(UserSettings settings)
        {
            File.WriteAllText(_PathSetting,
                JsonConvert.SerializeObject(settings));
        }

        public virtual void Dispose()
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