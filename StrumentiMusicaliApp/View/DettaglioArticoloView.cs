using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.Events.Image;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Core;
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
	public partial class DettaglioArticoloView : UserControl, IMenu
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
		private StrumentiMusicali.Library.Entity.Articolo _articolo = null;
		private static List<CategoriaItem> _categoriList = new List<CategoriaItem>();
		private FotoArticolo _fotoArticoloSelected = null;
		private List<PictureBox> _imageList = new List<PictureBox>();
		private string _lastFilter = "";
		private bool modeEdit = false;
		private System.Windows.Forms.PictureBox pb = new PictureBox();

		public DettaglioArticoloView()
			: base()
		{
			InitializeComponent();
			if (DesignMode)
				return;
			if (_articolo == null)
			{
				_articolo = new StrumentiMusicali.Library.Entity.Articolo() { Testo = "Prova", Titolo = "titolo", Marca = "PRova" };
			}
			EventAggregator.Instance().Subscribe<ImageListUpdate>(RefreshImageList);


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
		}

		public DettaglioArticoloView(ArticoloItem articolo)
			: this()
		{
			_articolo = articolo.ArticoloCS;
			modeEdit = true;
		}

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
				new ImageOrderSet(enOperationOrder.AumentaPriorita, _fotoArticoloSelected));
		}

		private void ChkPrezzoARichiesta_CheckedChanged(object sender, EventArgs e)
		{
			UpdateViewPrezzi();
		}

		private void DiminuisciPrioritàToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImageOrderSet>(
				new ImageOrderSet(enOperationOrder.DiminuisciPriorita, _fotoArticoloSelected));
		}

		private void FillCombo()
		{
			UpdateListCategory();

			cboCategoria.DataSource = _categoriList;
			cboCategoria.DisplayMember = "Descrizione";
			cboCategoria.ValueMember = "ID";

			var listReparti = _categoriList.Select(a => new
			{
				ID = a.Reparto,
				Descrizione = a.Reparto
			}).Distinct().ToList();
			cboReparto.DataSource = listReparti;
			cboReparto.DisplayMember = "Descrizione";
			cboReparto.ValueMember = "ID";
			cboReparto.SelectedIndexChanged += CboReparto_SelectedIndexChanged;

			using (var uof = new UnitOfWork())
			{
				txtMarca.Values = uof.ArticoliRepository.Find(a => true).Select(a => a.Marca).Distinct().ToList().ToArray();
			}

			cboCondizione.DataSource = Enum.GetNames(typeof(enCondizioneArticolo))
				.Select(a => new
				{
					ID = (enCondizioneArticolo)Enum.Parse(typeof(enCondizioneArticolo), a),
					Descrizione = a
				}).ToList();
			cboCondizione.DisplayMember = "Descrizione";
			cboCondizione.ValueMember = "ID";
		}

		private static void UpdateListCategory()
		{
			using (var uof = new UnitOfWork())
			{
				if (_categoriList.Count() == 0)
				{
					_categoriList = uof.CategorieRepository.Find(a => true).Select(a => new CategoriaItem
					{
						ID = a.ID,
						Descrizione = a.Categoria + " {" + a.Reparto + "}",
						Reparto = a.Reparto
					}).ToList();
				}
			}
		}

		private void CboReparto_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboCategoria.DataSource = _categoriList.FindAll(a => a.Reparto == cboReparto.Text).ToList();
			cboCategoria.Refresh();
		}

		private void frmArticolo_Load(object sender, EventArgs e)
		{
			if (DesignMode)
				return;
			txtFiltroCategoria.TextChanged += txtFiltroCategoria_TextChanged;

			this.tabControl1.DrawItem +=
						 new DrawItemEventHandler(PageTab_DrawItem);
			this.tabControl1.Selecting +=
				new TabControlCancelEventHandler(PageTab_Selecting);
			if (modeEdit == false)
			{
				_articolo = new StrumentiMusicali.Library.Entity.Articolo();
			}

			FillCombo();
			UpdateButtonState();

			chkPrezzoARichiesta.CheckedChanged += ChkPrezzoARichiesta_CheckedChanged;

			UtilityView.SetDataBind(this, _articolo);
			using (var uof = new UnitOfWork())
			{
				var giacenza = uof.MagazzinoRepository.Find(a => a.ArticoloID == _articolo.ID)
					.Select(a => a.Qta).DefaultIfEmpty(0).Sum(a => a);

				txtGiacenza.Value = giacenza;
			}
			if (cboCategoria.SelectedItem != null)
			{
				//seleziono reparto
				var item = ((CategoriaItem)cboCategoria.SelectedItem);
				cboReparto.Text = item.Reparto;
				cboCategoria.SelectedItem = item;
			}
		}

		private void FrmArticolo_ResizeEnd(object sender, EventArgs e)
		{
			ResizeImage();
		}

		private void MenuImpostaPrincipale_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImageOrderSet>(
				new ImageOrderSet(enOperationOrder.ImpostaPrincipale, _fotoArticoloSelected));
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
					new ImageAddFiles(_articolo, list));

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
				var imageList = uof.FotoArticoloRepository.Find(a => a.Articolo.ID == _articolo.ID)
					.OrderBy(a => a.Ordine).ToList();

				foreach (var item in imageList)
				{
					try
					{
						var itemPhoto = System.IO.Directory.GetFiles(ControllerImmagini.FolderFoto,
							item.UrlFoto).FirstOrDefault();
						if (itemPhoto != null)
						{
							PictureBox pb = new PictureBox();
							pb.SizeMode = PictureBoxSizeMode.Zoom;
							pb.Load(itemPhoto);

							pb.Click += Pb_Click;
							pb.MouseClick += Pb_MouseClick;
							pb.MouseMove += Pb_MouseMove;

							pb.Padding = new Padding(10);

							PanelImage.Controls.Add(pb);
							/*nel tag salvo l'elemento entity*/
							pb.Tag = item;

							_imageList.Add(pb);
						}
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
			EventAggregator.Instance().Publish<ImageRemove>(new ImageRemove(_fotoArticoloSelected));
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedTab == tabPage2)
			{
				RefreshImageListData();
			}
			UpdateButtonState();
		}

		private void txtFiltroCategoria_TextChanged(object sender, EventArgs e)
		{
			var text = txtFiltroCategoria.Text;
			if (_lastFilter == text)
			{ return; }

			if (text == string.Empty || text == null)
			{
				cboCategoria.DataSource = _categoriList; // cmbItems is a List of ComboBoxItem with some random numbers
				cboCategoria.SelectedIndex = -1;
				cboCategoria.MaxDropDownItems = 10;
			}
			else
			{
				string tempStr = text;

				var data = (from m in _categoriList where m.Descrizione.ToLower().Contains(tempStr.ToLower()) select m).ToList();

				cboCategoria.DataSource = data;

				cboCategoria.DroppedDown = true;
				//Cursor.Current = Cursors.Default;
				cboCategoria.SelectedIndex = -1;
				if (data.Count() < 10 && data.Count() > 0)
					cboCategoria.MaxDropDownItems = data.Count();

				_lastFilter = text;
			}
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
			tabPage2.Enabled = _fotoArticoloSelected != null; 
			if (_ribPannelImmagini != null)
			{
				_ribPannelImmagini.Enabled = tabControl1.SelectedTab == tabPage2;

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

		private void UpdateViewPrezzi()
		{
			txtPrezzo.Enabled = !(chkPrezzoARichiesta.Checked);
			txtPrezzoBarrato.Enabled = !(chkPrezzoARichiesta.Checked);
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

				_ribPannelImmagini = tab.Add("Immagini");

				var ribAdd = _ribPannelImmagini.Add("Aggiungi",
					Properties.Resources.Add);

				ribAdd.Click += (a, e) =>
				{
					EventAggregator.Instance().Publish<ImageAdd>(new ImageAdd(_articolo));
				};

				_ribRemove = _ribPannelImmagini.Add(
					"Rimuovi",
					Properties.Resources.Remove
				);
				_ribRemove.Click += (a, e) =>
				{
					EventAggregator.Instance().Publish<ImageRemove>(new ImageRemove(_fotoArticoloSelected));
				};

				var ribPnlSalva = tab.Add("Azioni");

				var ribSave = ribPnlSalva.Add("Salva", Properties.Resources.Save);
				ribSave.Click += (a, e) =>
				{
					this.txtID.Focus();
					this.Validate();
					EventAggregator.Instance().Publish<ArticoloSave>(new ArticoloSave(_articolo));
					UpdateButtonState();
				};
			}
			return _menuTab;
		}
	}
}