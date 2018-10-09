using StrumentiMusicaliSql.Repo;
using SturmentiMusicaliApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SturmentiMusicaliApp.Forms
{
	public partial class frmArticolo : Form
	{
		ArticoloItem _articolo = new ArticoloItem();
		public frmArticolo()
			:base()
		{
			InitializeComponent();
			ribSave.Click += RibSave_Click;
		}

		private void RibSave_Click(object sender, EventArgs e)
		{
			this.Validate();
			using (var uof =new UnitOfWork())
			{
				uof.ArticoliRepository.Update(_articolo.ArticoloCS);
				uof.Commit();
			}
		}

		 

		public frmArticolo(ArticoloItem articolo )
			:this()
		{
			_articolo = articolo;

			
			
		}
		
		private void frmArticolo_Load(object sender, EventArgs e)
		{
			var listControlWithTag = FindControlByType<Control>(this).Where(a => a.Tag != null && a.Tag.ToString().Length>0);


			foreach (var item in GetProperties(_articolo.ArticoloCS))
			{
				var listByTag = listControlWithTag.Where(a => a.Tag.ToString() == item.Name);

				foreach (var cnt in listByTag)
				{
					if (cnt is TextBox)
					{
						cnt.DataBindings.Add("Text", _articolo.ArticoloCS, item.Name);
					}
					else if (cnt is NumericUpDown)
					{
						cnt.DataBindings.Add("Value", _articolo.ArticoloCS, item.Name);
					}
					else if (cnt is CheckBox)
					{
						cnt.DataBindings.Add("Checked", _articolo.ArticoloCS, item.Name);
					}

				}
				 
			}
			using (var uof =new UnitOfWork())
			{
				txtCategoria.Values = uof.CategorieRepository.Find(a => true).Select(a => a.ID.ToString() + " " +  a.Categoria +  " {" + a.Reparto + "}").ToList().ToArray();
			}
			
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
