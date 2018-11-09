using StrumentiMusicali.App.Core.Controllers.Imports;
using System;
using System.Windows.Forms;

namespace testForm
{
	internal static class Program
	{
		/// <summary>
		/// Punto di ingresso principale dell'applicazione.
		/// </summary>
		[STAThread]
		private static void Main()
		{

			var prova = new ImportMagazziniExcel();
			prova.ImportFile(@"C:\Users\fastcode13042017\Desktop\StrumentiM\MAGAZZINO.xls");

			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());
		}
	}
}