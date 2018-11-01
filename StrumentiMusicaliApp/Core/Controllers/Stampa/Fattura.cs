using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Stampa
{
	class StampaFattura
	{
		public	void Stampa()
		{
			var excel = new ClosedXML.Excel.XLWorkbook("");
			excel.Range("Righe").Range(2, 1, 2, 1).Value = "Codice ALE";
			excel.Range("Righe").Range(2, 3, 2, 3).Value = "Codice ALE 3";
			var newfile = Path.Combine(Application.StartupPath, "Ale.xlsx");
			excel.SaveAs(newfile);
			Process.Start(newfile);
		}
	}
}
