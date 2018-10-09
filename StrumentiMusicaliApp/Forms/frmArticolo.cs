using StrumentiMusicaliSql.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SturmentiMusicaliApp.Forms
{
	public partial class frmArticolo : Form
	{
		public frmArticolo()
		{
			InitializeComponent();
		}

		private void frmArticolo_Load(object sender, EventArgs e)
		{
			using (var uof =new UnitOfWork())
			{
				txtCategoria.Values = uof.CategorieRepository.Find(a => true).Select(a => a.ID.ToString() + " " +  a.Categoria +  " {" + a.Reparto + "}").ToList().ToArray();
			}
			
		}
	}
}
