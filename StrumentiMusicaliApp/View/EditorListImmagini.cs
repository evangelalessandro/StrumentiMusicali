using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
    public partial class EditorListImmagini<K> : UserControl
        where K : BaseEntity
    {
        private ImmaginiFile<K> _fileSelected = null;
        protected DragDropEffects effect;
        protected Thread getImageThread;
        protected Image image;
        protected string lastFilename = String.Empty;
        protected int lastX = 0;
        protected int lastY = 0;
        protected Image nextImage;
        protected PictureBox thumbnail;
        private List<PictureBox> _imageList = new List<PictureBox>();

        private PictureBox pb = new PictureBox();
        /*controller riferimento eventi*/
        IKeyController _KeyController = null;
        public EditorListImmagini(IKeyController keyController)
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.BackColor = Color.Green;

            _KeyController = keyController;

            diminuisciPrioritàToolStripMenuItem.Click += DiminuisciPrioritàToolStripMenuItem_Click;
            aumentaPrioritàToolStripMenuItem.Click += AumentaPrioritàToolStripMenuItem_Click;
            menuImpostaPrincipale.Click += MenuImpostaPrincipale_Click;
            rimuoviImmagineToolStripMenuItem.Click += RimuoviImmagineToolStripMenuItem_Click;

            PanelImage.AllowDrop = true;

            PanelImage.DragEnter += new DragEventHandler(PanelImage_DragEnter);
            PanelImage.DragDrop += new DragEventHandler(PanelImage_DragDrop);
            PanelImage.DragLeave += PanelImage_DragLeave;

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

            this.Resize += udc_ResizeEnd;
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
        private void udc_ResizeEnd(object sender, EventArgs e)
        {
            ResizeImage();
        }

        /// <summary>
        /// Lista immagini
        /// </summary>
        /// <param name="fileList"></param>
        public void RefreshImageListData(List<ImmaginiFile<K>> fileList)
        {
            PanelImage.AutoScroll = true;

            foreach (Control itemCrt in _imageList)
            {
                PanelImage.Controls.Remove(itemCrt);
                itemCrt.Dispose();
            }
            _imageList.Clear();


            foreach (var item in fileList)
            {
                try
                {
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    var extension = new FileInfo(item.File).Extension;
                    if (!System.IO.File.Exists(item.File))
                    {
                        pb.Image = Properties.Resources.ImmagineMancante;
                        MessageManager.NotificaWarnig("Manca l'immagine " +
                        item.File);

                    }
                    else if (extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                    {
                        pb.Image = Properties.Resources.pdfIcon;
                    }
                    else if (IsImage(extension))
                    {
                        pb.Load(item.File);

                    }
                    else
                    {
                        pb.Image = Properties.Resources.PreviewIcon;
                    }
                    pb.Click += Pb_Click;
                    pb.MouseClick += Pb_MouseClick;
                    pb.DoubleClick += Pb_DoubleClick;
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
                FileSelezionato = null;
                UpdateButtonState();
            }

        }
        public bool IsImage(string extension)
        {

            return (extension.Equals(".jpg",
                        StringComparison.InvariantCultureIgnoreCase)
                        ||
                        extension.Equals(".jpeg",
                        StringComparison.InvariantCultureIgnoreCase)
                        ||
                        extension.Equals(".bmp",
                        StringComparison.InvariantCultureIgnoreCase)
                         ||
                        extension.Equals(".png",
                        StringComparison.InvariantCultureIgnoreCase)
                        );

        }
        private void Pb_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(this._fileSelected.File);
        }

        public ImmaginiFile<K> FileSelezionato {
            get {
                return _fileSelected;
            }
            private set {
                _fileSelected = value;
                EventAggregator.Instance().Publish<ImageSelected<K>>(new ImageSelected<K>(value, _KeyController.INSTANCE_KEY));
            }
        }
        public void UpdateButtonState()
        {

            this.rimuoviImmagineToolStripMenuItem.Visible = FileSelezionato != null;
        }
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
        public delegate void AssignImageDlgt();



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
                        if ((IsImage(ext) || (ext == ".pdf")))
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

        protected bool validData;
        private void ResizeImage()
        {
            foreach (var item in _imageList)
            {
                item.Height = this.Size.Height / 21 * 10;
                item.Width = this.Size.Width / 21 * 10;
            }
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
                    new ImageAddFiles(
                    list, _KeyController.INSTANCE_KEY));



                if (pb.Image != null)
                    pb.Image.Dispose();
                Debug.Print("pb.Visible = false");
                pb.Visible = false;
            }

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

        private void Pb_Click(object sender, EventArgs e)
        {
            UpdateColor();
            foreach (var item in _imageList)
            {
                if (item == sender)
                {
                    item.BackColor = System.Drawing.Color.Orange;
                    FileSelezionato = (ImmaginiFile<K>)item.Tag;
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

        private void PanelImage_DragLeave(object sender, EventArgs e)
        {
            Debug.Print("PanelImage_DragLeave");
            thumbnail.Visible = false;
        }
        private void AumentaPrioritàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageOrderSet<K>>(
                new ImageOrderSet<K>(enOperationOrder.AumentaPriorita,
                FileSelezionato, _KeyController.INSTANCE_KEY));
        }

        private void RimuoviImmagineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageRemove<K>>(
                new ImageRemove<K>(FileSelezionato,
                _KeyController.INSTANCE_KEY));
        }
        private void MenuImpostaPrincipale_Click(object sender, EventArgs e)
        {

            EventAggregator.Instance().Publish<ImageOrderSet<K>>(
                new ImageOrderSet<K>(enOperationOrder.ImpostaPrincipale,
                FileSelezionato,
                _KeyController.INSTANCE_KEY));
        }
        private void DiminuisciPrioritàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<ImageOrderSet<K>>(
                new ImageOrderSet<K>(enOperationOrder.DiminuisciPriorita,
                FileSelezionato,
                _KeyController.INSTANCE_KEY));
        }

    }
}
