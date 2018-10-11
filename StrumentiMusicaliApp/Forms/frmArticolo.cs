using StrumentiMusicaliApp.Core;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Entity;
using StrumentiMusicaliSql.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Forms
{
	public partial class frmArticolo : Form
	{
		private bool modeEdit = false;
		private StrumentiMusicaliSql.Entity.Articolo _articolo = null;
		public frmArticolo()
			: base()
		{
			InitializeComponent();
			if (_articolo == null)
			{
				_articolo = new StrumentiMusicaliSql.Entity.Articolo() { Testo = "Prova", Titolo = "titolo", Marca = "PRova" };

			}

			ribSave.Click += RibSave_Click;
		}

		private void RibSave_Click(object sender, EventArgs e)
		{
			this.txtID.Focus();
			this.Validate();
			try
			{
				using (var curs = new CursorHandler())
				{
					using (var uof = new UnitOfWork())
					{
						if (modeEdit)
						{
							uof.ArticoliRepository.Update(_articolo);
						}
						else
						{
							uof.ArticoliRepository.Add(_articolo);
						}
						uof.Commit();
					}
				}
				MessageManager.NotificaInfo("Salvataggio avvenuto con successo");

			}
			catch (MessageException ex)
			{

				MessageBox.Show(ex.Messages);
			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);
			}

		}




		public frmArticolo(ArticoloItem articolo)
			: this()
		{
			_articolo = articolo.ArticoloCS;
			modeEdit = true;



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
						,Descrizione = a
					}).ToList();
				cboCondizione.DisplayMember = "Descrizione";
				cboCondizione.ValueMember = "ID";

			}


		}

		private List<CategoriaItem> _categoriList = new List<CategoriaItem>();
		private string _lastFilter = "";
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



		private void ChkPrezzoARichiesta_CheckedChanged(object sender, EventArgs e)
		{
			UpdateViewPrezzi();
		}

		private void UpdateViewPrezzi()
		{
			txtPrezzo.Enabled = !(chkPrezzoARichiesta.Checked);
			txtPrezzoBarrato.Enabled = !(chkPrezzoARichiesta.Checked);
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


	}
}
