using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Forms
{
    public partial class DettaglioArticoloView : UserControl, IMenu, ICloseSave
    {


        private static List<CategoriaItem> _categoriList = new List<CategoriaItem>();

        private EditorListImmagini<FotoArticolo> _EditorListImmagini;
        Subscription<ImageSelected<FotoArticolo>> _sub12;
        private Subscription<ImageListUpdate> _sub1;
        public DettaglioArticoloView(ControllerArticoli articoloController)
            : base()
        {
            _articoloController = articoloController;
            InitializeComponent();
            if (DesignMode)
                return;
            _sub12 = EventAggregator.Instance().Subscribe<ImageSelected<FotoArticolo>>(ImmagineSelezionata);

            tabPage2.AllowDrop = true;
            _EditorListImmagini = new EditorListImmagini<FotoArticolo>(_articoloController);

            _sub1 = EventAggregator.Instance().Subscribe<ImageListUpdate>(RefreshImageList);

            _EditorListImmagini.Dock = DockStyle.Fill;
            tabPage2.Controls.Add(_EditorListImmagini);


            /*controlla se mostrare libro o no*/
            if (!articoloController.EditItem.ShowLibro.HasValue)
            {
                articoloController.EditItem.ShowLibro =
                    articoloController.EditItem.IsLibro();

            }
            articoloController.EditItem.ShowStrumento = !articoloController.EditItem.ShowLibro;

            _EditView = new View.Settings.GenericSettingView(articoloController.EditItem);
            _EditView.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(_EditView);
            tabPage1.AutoScroll = true;
            AggiornaQtaNegozio();


        }

        private void ImmagineSelezionata(ImageSelected<FotoArticolo> obj)
        {
            UpdateButtonState();
        }

        private void AggiornaQtaNegozio()
        {
            if (_articoloController.EditItem.ID != 0)
            {
                using (var uof = new UnitOfWork())
                {
                    int idArt = _articoloController.EditItem.ID;
                    var giacenza = uof.MagazzinoRepository.Find(a => a.ArticoloID == idArt)
                          .Select(a => new { a.ArticoloID, a.Qta, a.Deposito }).GroupBy(a => new { a.ArticoloID, a.Deposito })
                          .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key }).ToList();



                    var val = giacenza.Where(a => a.Articolo.ArticoloID == idArt).ToList();
                    //.Select(a => a.Sum).FirstOrDefault();

                    _articoloController.EditItem.QtaNegozio = val.Where(a => a.Articolo.Deposito.Principale).Select(a => a.Sum)
                                .DefaultIfEmpty(0).FirstOrDefault();

                    //item.QuantitaTotale = val.Sum(a => a.Sum);
                    _EditView.Validate();
                    _EditView.Update();

                }
            }
        }
        private View.Settings.GenericSettingView _EditView;

        private ControllerArticoli _articoloController;

        private void frmArticolo_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            this.tabControl1.DrawItem +=
                         new DrawItemEventHandler(PageTab_DrawItem);
            this.tabControl1.Selecting +=
                new TabControlCancelEventHandler(PageTab_Selecting);

            UpdateButtonState();


            UtilityView.SetDataBind(this, null, _articoloController.EditItem);

        }


        private void RefreshImageList(ImageListUpdate obj)
        {
            RefreshImageListData();
        }

        private void RefreshImageListData()
        {
            _EditorListImmagini.RefreshImageListData(_articoloController.RefreshImageListData());
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                RefreshImageListData();
            }
            UpdateButtonState();
        }


        /// <summary>
        /// Draw a tab page based on whether it is disabled or enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage tabPage = tabControl.TabPages[e.Index];

            if (tabPage.Enabled == false)
            {
                using (SolidBrush brush =
                   new SolidBrush(SystemColors.GrayText))
                {
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, brush,
                       e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(tabPage.ForeColor))
                {
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, brush,
                       e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
        }

        /// <summary>
        /// Cancel the selecting event if the TabPage is disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageTab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Enabled == false)
            {
                e.Cancel = true;
            }
        }

        private void UpdateButtonState()
        {
            tabPage2.Enabled = _articoloController.EditItem != null
                && _articoloController.EditItem.ID != 0;
            if (_ribPannelImmagini != null)
            {
                _ribPannelImmagini.Enabled = tabControl1.SelectedTab == tabPage2;
                _ribRemove.Enabled = true;
                _ribRemove.Enabled = _articoloController.FotoSelezionata() != null;
            }
            _EditorListImmagini.UpdateButtonState();
        }



        private MenuTab _menuTab = null;
        private RibbonMenuPanel _ribPannelImmagini;
        private RibbonMenuButton _ribRemove;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            EventAggregator.Instance().UnSbscribe(_sub12);

            EventAggregator.Instance().UnSbscribe(_sub1);

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        public MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                var tab = _menuTab.Add("Principale");

                var ribPnlSalva = tab.Add("Azioni");

                UtilityView.AddButtonSaveAndClose(ribPnlSalva, this);
                _ribPannelImmagini = tab.Add("Immagini");

                var ribAdd = _ribPannelImmagini.Add("Aggiungi",
                    Properties.Resources.Add);

                ribAdd.Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ImageArticoloAdd>(
                        new ImageArticoloAdd(_articoloController.EditItem, _articoloController));
                };

                _ribRemove = _ribPannelImmagini.Add(
                    "Rimuovi",
                    Properties.Resources.Remove
                );
                _ribRemove.Click += (b, e) =>
                {
                    FotoArticolo foto = _articoloController.FotoSelezionata();

                    EventAggregator.Instance().Publish<ImageArticoloRemove>(
                        new ImageArticoloRemove(foto,
                        _articoloController));
                };
                var magazzino = tab.Add("Magazzino");

                var rib4 = magazzino.Add("1 pezzo venduto", Properties.Resources.Remove, true);
                rib4.Click += (a, e) =>
                {
                    if (_articoloController.EditItem.ID == 0)
                    {
                        MessageManager.NotificaWarnig(@"Occorre prima salvare l'articolo\libro per poter aggiungere qta.");
                        return;
                    }
                    using (var depo = new Core.Controllers.ControllerMagazzino(_articoloController.EditItem))
                    {
                        ScaricaQtaMagazzino scarica = new ScaricaQtaMagazzino();

                        scarica.Qta = 1;

                        using (var uof = new UnitOfWork())
                        {
                            var principale = uof.DepositoRepository.Find(b => b.Principale).FirstOrDefault();
                            if (principale == null)
                            {
                                MessageManager.NotificaWarnig("Occorre impostare un deposito principale, vai in depositi e imposta il deposito principale.");
                                return;
                            }
                            scarica.Deposito = principale.ID;
                            scarica.ArticoloID = _articoloController.EditItem.ID;
                            EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(scarica);
                            AggiornaQtaNegozio();
                        }
                    }

                };
                var rib5 = magazzino.Add("1 pezzo aggiunto", Properties.Resources.Add, true);
                rib5.Click += (a, e) =>
                {
                    if (_articoloController.EditItem.ID == 0)
                    {
                        MessageManager.NotificaWarnig(@"Occorre prima salvare l'articolo\libro per poter aggiungere qta.");
                        return;
                    }
                    using (var depo = new Core.Controllers.ControllerMagazzino(_articoloController.EditItem))
                    {
                        var carica = new CaricaQtaMagazzino();

                        carica.Qta = 1;

                        using (var uof = new UnitOfWork())
                        {
                            var principale = uof.DepositoRepository.Find(b => b.Principale).FirstOrDefault();
                            if (principale == null)
                            {
                                MessageManager.NotificaWarnig("Occorre impostare un deposito principale, vai in depositi e imposta il deposito principale.");
                                return;
                            }
                            carica.Deposito = principale.ID;
                            carica.ArticoloID = _articoloController.EditItem.ID;
                            EventAggregator.Instance().Publish<CaricaQtaMagazzino>(carica);
                            AggiornaQtaNegozio();
                        }
                    }
                };


            }
            return _menuTab;
        }



        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        public void RaiseSave()
        {
            this.Validate();
            EventAggregator.Instance().Publish<Save<Articolo>>(
                new Save<Articolo>(_articoloController));
            UpdateButtonState();
        }

        public void RaiseClose()
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs());
            }
        }
    }
}