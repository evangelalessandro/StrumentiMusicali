using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Forms
{
    public partial class DettaglioArticoloView : UserControl, IMenu, ICloseSave
    {
        protected DragDropEffects effect;
        protected Thread getImageThread;
        protected Image image;
        protected string lastFilename = String.Empty;
        protected int lastX = 0;
        protected int lastY = 0;
        protected Image nextImage;
        protected PictureBox thumbnail;
        protected bool validData;
        private static List<CategoriaItem> _categoriList = new List<CategoriaItem>();
        private FotoArticolo _fotoArticoloSelected = null;
        private List<PictureBox> _imageList = new List<PictureBox>();

        private System.Windows.Forms.PictureBox pb = new PictureBox();
        private SettingSito _settingSito = null;
        private Subscription<ImageListUpdate> _sub1;
        public DettaglioArticoloView(ControllerArticoli articoloController, SettingSito settingSito)
            : base()
        {
            _articoloController = articoloController;
            InitializeComponent();
            if (DesignMode)
                return;
            _settingSito = settingSito;
            _sub1 = EventAggregator.Instance().Subscribe<ImageListUpdate>(RefreshImageList);

            PanelImage.AllowDrop = true;

            PanelImage.DragEnter += new DragEventHandler(PanelImage_DragEnter);
            PanelImage.DragDrop += new DragEventHandler(PanelImage_DragDrop);
            PanelImage.DragLeave += PanelImage_DragLeave;
            diminuisciPrioritàToolStripMenuItem.Click += DiminuisciPrioritàToolStripMenuItem_Click;
            aumentaPrioritàToolStripMenuItem.Click += AumentaPrioritàToolStripMenuItem_Click;
            menuImpostaPrincipale.Click += MenuImpostaPrincipale_Click;
            rimuoviImmagineToolStripMenuItem.Click += RimuoviImmagineToolStripMenuItem_Click;
            thumbnail = new PictureBox();
            thumbnail.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Controls.Add(thumbnail);
            thumbnail.Visible = false;
            this.pb.Location = new System.Drawing.Point(0, 0);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(292, 266);
            this.pb.TabIndex = 0;
            this.pb.TabStop = false;
            this.pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Controls.Add(thumbnail);
            PanelImage.Controls.Add(pb);
            this.Resize += FrmArticolo_ResizeEnd;

            /*controlla se mostrare libro o no*/
            if (!articoloController.EditItem.ShowLibro.HasValue)
            {
                articoloController.EditItem.ShowLibro = (!string.IsNullOrEmpty(articoloController.EditItem.Libro.Autore)
                    || !string.IsNullOrEmpty(articoloController.EditItem.Libro.Edizione)
                    || !string.IsNullOrEmpty(articoloController.EditItem.Libro.Settore));
            }
            articoloController.EditItem.ShowStrumento = !articoloController.EditItem.ShowLibro;

            _EditView = new View.Settings.GenericSettingView(articoloController.EditItem);
            _EditView.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(_EditView);
            tabPage1.AutoScroll = true;
            AggiornaQtaNegozio();
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


        public delegate void AssignImageDlgt();

        protected void AssignImage()
        {
            thumbnail.Width = 100;
            // 100    iWidth
            // ---- = ------
            // tHeight  iHeight
            thumbnail.Height = nextImage.Height * 100 / nextImage.Width;
            SetThumbnailLocation(this.PointToClient(new Point(lastX, lastY)));
            thumbnail.Image = nextImage;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            EventAggregator.Instance().UnSbscribe(_sub1);
            if (nextImage != null)
            {
                nextImage.Dispose();
            }
            nextImage = null;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;
            filename = String.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".jpeg") || (ext == ".bmp"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        protected void LoadImage()
        {
            nextImage = new Bitmap(lastFilename);
            this.Invoke(new AssignImageDlgt(AssignImage));
        }

        protected void SetThumbnailLocation(Point p)
        {
            if (thumbnail.Image == null)
            {
                thumbnail.Visible = false;
            }
            else
            {
                p.X -= thumbnail.Width / 2;
                p.Y -= thumbnail.Height / 2;
                thumbnail.Location = p;
                thumbnail.Visible = true;
            }
        }

        private void AumentaPrioritàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageOrderSet>(
                new ImageOrderSet(enOperationOrder.AumentaPriorita, _fotoArticoloSelected,_articoloController));
        }


        private void DiminuisciPrioritàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageOrderSet>(
                new ImageOrderSet(enOperationOrder.DiminuisciPriorita, _fotoArticoloSelected, _articoloController));
        }

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
        private void FrmArticolo_ResizeEnd(object sender, EventArgs e)
        {
            ResizeImage();
        }

        private void MenuImpostaPrincipale_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageOrderSet>(
                new ImageOrderSet(enOperationOrder.ImpostaPrincipale, _fotoArticoloSelected,_articoloController._controllerImmagini));
        }

        private void PanelImage_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("OnDragDrop");
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                thumbnail.Visible = false;
                image = nextImage;

                var list = new List<string>() { lastFilename };

                EventAggregator.Instance().Publish<ImageAddFiles>(
                    new ImageAddFiles(_articoloController.EditItem, list,_articoloController));

                //AdjustView();
                if (pb.Image != null)
                    pb.Image.Dispose();
                Debug.Print("pb.Visible = false");
                pb.Visible = false;
            }

            //PictureBox data = (PictureBox)e.Data.GetData(typeof(PictureBox));
            //Point p = PanelImage.PointToClient(new Point(e.X, e.Y));
            //var item = PanelImage.GetChildAtPoint(p);
            //int index = PanelImage.Controls.GetChildIndex(item, false);
            //PanelImage.Controls.SetChildIndex(data, index);
            //PanelImage.Invalidate();
        }

        private void PanelImage_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                Debug.Print("Valid data PanelImage_DragEnter");
                if (lastFilename != filename)
                {
                    thumbnail.Image = null;
                    thumbnail.Visible = false;
                    lastFilename = filename;
                    getImageThread = new Thread(new ThreadStart(LoadImage));
                    getImageThread.Start();
                }
                else
                {
                    thumbnail.Visible = true;
                }
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void PanelImage_DragLeave(object sender, EventArgs e)
        {
            Debug.Print("PanelImage_DragLeave");
            thumbnail.Visible = false;
        }

        private void Pb_Click(object sender, EventArgs e)
        {
            UpdateColor();
            foreach (var item in _imageList)
            {
                if (item == sender)
                {
                    item.BackColor = System.Drawing.Color.Orange;
                    _fotoArticoloSelected = (FotoArticolo)item.Tag;
                }
            }
        }

        private void Pb_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var relativeClickedPosition = e.Location;
                var screenClickedPosition = (sender as Control).PointToScreen(relativeClickedPosition);

                contextMenuStrip1.Show(screenClickedPosition);
            }
        }

        private void Pb_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void RefreshImageList(ImageListUpdate obj)
        {
            RefreshImageListData();
        }

        private void RefreshImageListData()
        {
            PanelImage.AutoScroll = true;

            foreach (Control itemCrt in _imageList)
            {
                PanelImage.Controls.Remove(itemCrt);
                itemCrt.Dispose();
            }
            _imageList.Clear();
            using (var uof = new UnitOfWork())
            {
                var imageList = uof.FotoArticoloRepository.Find(a => a.Articolo.ID == _articoloController.EditItem.ID)
                    .OrderBy(a => a.Ordine).ToList();


                foreach (var item in imageList)
                {
                    try
                    {
                        var itemPhoto = System.IO.Directory.GetFiles(
                            _settingSito.CartellaLocaleImmagini,
                            item.UrlFoto).FirstOrDefault();

                        PictureBox pb = new PictureBox();
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        if (itemPhoto == null)
                        {
                            pb.Image = Properties.Resources.ImmagineMancante;
                            MessageManager.NotificaWarnig("Manca l'immagine " +
                            Path.Combine(_settingSito.CartellaLocaleImmagini,
                            item.UrlFoto));

                        }
                        else
                        {
                            pb.Load(itemPhoto);
                        }
                        pb.Click += Pb_Click;
                        pb.MouseClick += Pb_MouseClick;
                        pb.MouseMove += Pb_MouseMove;

                        pb.Padding = new Padding(10);

                        PanelImage.Controls.Add(pb);
                        /*nel tag salvo l'elemento entity*/
                        pb.Tag = item;

                        _imageList.Add(pb);

                        UpdateColor();
                        ResizeImage();
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.ManageError(ex, true);
                    }
                    _fotoArticoloSelected = null;
                    UpdateButtonState();
                }
            }
        }

        private void ResizeImage()
        {
            foreach (var item in _imageList)
            {
                item.Height = tabControl1.Size.Height / 21 * 10;
                item.Width = tabControl1.Size.Width / 21 * 10;
            }
        }

        private void RimuoviImmagineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageRemove>(new ImageRemove(_fotoArticoloSelected,_articoloController));
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
                _ribRemove.Enabled = _fotoArticoloSelected != null;
            }
            this.rimuoviImmagineToolStripMenuItem.Visible = _fotoArticoloSelected != null;
        }

        private void UpdateColor()
        {
            bool first = true;
            foreach (var item in _imageList)
            {
                if (first)
                {
                    item.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    item.BackColor = System.Drawing.Color.Green;
                }
                first = false;
            }
        }

        private MenuTab _menuTab = null;
        private RibbonMenuPanel _ribPannelImmagini;
        private RibbonMenuButton _ribRemove;


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
                    EventAggregator.Instance().Publish<ImageAdd>(
                        new ImageAdd(_articoloController.EditItem, _articoloController));
                };

                _ribRemove = _ribPannelImmagini.Add(
                    "Rimuovi",
                    Properties.Resources.Remove
                );
                _ribRemove.Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ImageRemove>(new ImageRemove(_fotoArticoloSelected,_articoloController));
                };
                var magazzino= tab.Add("Magazzino");

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
                    if (_articoloController.EditItem.ID==0)
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