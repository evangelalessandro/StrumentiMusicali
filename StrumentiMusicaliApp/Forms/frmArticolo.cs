using StrumentiMusicaliApp.Core;
using StrumentiMusicaliApp.Core.Events;
using StrumentiMusicaliApp.Core.Events.Image;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Entity;
using StrumentiMusicaliSql.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Forms
{
	public partial class frmArticolo : Form
	{
		private StrumentiMusicaliSql.Entity.Articolo _articolo = null;
		private List<CategoriaItem> _categoriList = new List<CategoriaItem>();
		private List<PictureBox> _imageList = new List<PictureBox>();
		private string _lastFilter = "";
		private bool modeEdit = false;
		public frmArticolo()
			: base()
		{
			InitializeComponent();
			if (_articolo == null)
			{
				_articolo = new StrumentiMusicaliSql.Entity.Articolo() { Testo = "Prova", Titolo = "titolo", Marca = "PRova" };
			}
			EventAggregator.Instance().Subscribe<ImageListUpdate>(RefreshImageList);

			ribSave.Click += RibSave_Click;
			PanelImage.AllowDrop = true;

			PanelImage.DragEnter += new DragEventHandler(PanelImage_DragEnter);
			PanelImage.DragDrop += new DragEventHandler(PanelImage_DragDrop);

			diminuisciPrioritàToolStripMenuItem.Click += DiminuisciPrioritàToolStripMenuItem_Click;
			aumentaPrioritàToolStripMenuItem.Click += AumentaPrioritàToolStripMenuItem_Click;
			menuImpostaPrincipale.Click += MenuImpostaPrincipale_Click;
		}

		private void RefreshImageList(ImageListUpdate obj)
		{
			RefreshImageList();
		}

		public frmArticolo(ArticoloItem articolo)
			: this()
		{
			_articolo = articolo.ArticoloCS;
			modeEdit = true;
		}

		public static List<T> FindControlByType<T>(Control mainControl, bool getAllChild = true) where T : Control
		{
			List<T> lt = new List<T>();
			for (int i = 0; i < mainControl.Controls.Count; i++)
			{
				if (mainControl.Controls[i] is T) lt.Add((T)mainControl.Controls[i]);
				if (getAllChild) lt.AddRange(FindControlByType<T>(mainControl.Controls[i], getAllChild));
			}
			return lt;
		}

		public IEnumerable<PropertyInfo> GetProperties(Object obj)
		{
			Type t = obj.GetType();

			return t.GetProperties()
				.Where(p => (p.Name != "EntityKey" && p.Name != "EntityState"))
				.Select(p => p).ToList();
		}

		private void AumentaPrioritàToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImageOrderSet>(
				new ImageOrderSet(enOperationOrder.AumentaPriorita,_fotoArticoloSelected));
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
			using (var uof = new UnitOfWork())
			{
				_categoriList = uof.CategorieRepository.Find(a => true).Select(a => new CategoriaItem
				{
					ID = a.ID,
					Descrizione = a.Categoria + " {" + a.Reparto + "}"
				}).ToList();
				cboCategoria.DataSource = _categoriList;
				cboCategoria.DisplayMember = "Descrizione";
				cboCategoria.ValueMember = "ID";

				txtMarca.Values = uof.ArticoliRepository.Find(a => true).Select(a => a.Marca).Distinct().ToList().ToArray();

				cboCondizione.DataSource = Enum.GetNames(typeof(enCondizioneArticolo))
					.Select(a => new
					{
						ID = (enCondizioneArticolo)Enum.Parse(typeof(enCondizioneArticolo), a)
						,
						Descrizione = a
					}).ToList();
				cboCondizione.DisplayMember = "Descrizione";
				cboCondizione.ValueMember = "ID";
			}
		}

		private void frmArticolo_Load(object sender, EventArgs e)
		{
			if (modeEdit == false)
			{
				_articolo = new StrumentiMusicaliSql.Entity.Articolo();
			}

			FillCombo();

			var listControlWithTag = FindControlByType<Control>(this).Where(a => a.Tag != null && a.Tag.ToString().Length > 0);

			chkPrezzoARichiesta.CheckedChanged += ChkPrezzoARichiesta_CheckedChanged;

			foreach (var item in GetProperties(_articolo))
			{
				var listByTag = listControlWithTag.Where(a => a.Tag.ToString() == item.Name);

				foreach (var cnt in listByTag)
				{
					if (cnt is TextBox)
					{
						cnt.DataBindings.Add("Text", _articolo, item.Name);
					}
					else if (cnt is NumericUpDown)
					{
						cnt.DataBindings.Add("Value", _articolo, item.Name);
					}
					else if (cnt is CheckBox)
					{
						cnt.DataBindings.Add("Checked", _articolo, item.Name);
					}
					else if (cnt is ComboBox)
					{
						cnt.DataBindings.Add("SelectedValue", _articolo, item.Name);
					}
				}
			}
		}

		private void MenuImpostaPrincipale_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImageOrderSet>(
				new ImageOrderSet(enOperationOrder.ImpostaPrincipale, _fotoArticoloSelected));
		}
		private void PanelImage_DragDrop(object sender, DragEventArgs e)
		{
			PictureBox data = (PictureBox)e.Data.GetData(typeof(PictureBox));
			Point p = PanelImage.PointToClient(new Point(e.X, e.Y));
			var item = PanelImage.GetChildAtPoint(p);
			int index = PanelImage.Controls.GetChildIndex(item, false);
			PanelImage.Controls.SetChildIndex(data, index);
			PanelImage.Invalidate();
		}

		private void PanelImage_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}
		FotoArticolo _fotoArticoloSelected = null;
		private void Pb_Click(object sender, EventArgs e)
		{
			foreach (var item in _imageList)
			{
				item.BackColor = System.Drawing.Color.Green;
				if (item == sender)
				{
					item.BackColor = System.Drawing.Color.Orange;
					_fotoArticoloSelected = (FotoArticolo) item.Tag;
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

		private void RefreshImageList()
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
					.OrderBy(a=>a.Ordine).ToList();
				foreach (var item in imageList)
				{
					try
					{
						var itemPhoto = System.IO.Directory.GetFiles(Controller.FolderFoto, item.UrlFoto).FirstOrDefault();
						if (itemPhoto != null)
						{
							PictureBox pb = new PictureBox();
							pb.SizeMode = PictureBoxSizeMode.AutoSize;
							pb.Load(itemPhoto);
							pb.Click += Pb_Click;
							pb.MouseClick += Pb_MouseClick;
							pb.MouseMove += Pb_MouseMove;

							pb.Padding = new Padding(10);
							pb.BackColor = System.Drawing.Color.Green;
							PanelImage.Controls.Add(pb);
							/*nel tag salvo l'elemento entity*/
							pb.Tag = item;

							_imageList.Add(pb);
						}
					}
					catch (Exception ex)
					{
						ExceptionManager.ManageError(ex, true);
					}
				}
			}
		}

		private void ribAddImage_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImageAdd>(new ImageAdd(_articolo));
		}

		private void ribRemoveImage_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<ImageRemove>(new ImageRemove());
		}

		private void RibSave_Click(object sender, EventArgs e)
		{
			this.txtID.Focus();
			this.Validate();
			EventAggregator.Instance().Publish<ArticoloSave>(new ArticoloSave(_articolo));
		}
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedTab == tabPage2)
				RefreshImageList();
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
		private void UpdateViewPrezzi()
		{
			txtPrezzo.Enabled = !(chkPrezzoARichiesta.Checked);
			txtPrezzoBarrato.Enabled = !(chkPrezzoARichiesta.Checked);
		}
	}
}