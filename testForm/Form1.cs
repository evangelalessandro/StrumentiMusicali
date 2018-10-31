using StrumentiMusicali.App.Core.Controllers.Imports;
using System;
using System.Windows.Forms;

namespace testForm
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var prova = new ImportMagazziniExcel();
			prova.SelectFile();
		}
	}
}