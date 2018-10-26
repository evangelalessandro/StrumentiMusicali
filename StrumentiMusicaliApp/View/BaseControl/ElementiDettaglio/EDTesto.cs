using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrumentiMusicali.App.CustomComponents;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDTesto : EDBase
	{
		public EDTesto():
			base()
		{
			InitializeComponent();

			ControlToBind = new AutoCompleteTextBox();
			panel1.Controls.Add(ControlToBind);
			ControlToBind.Dock = DockStyle.Fill;
			ControlToBind.BringToFront();
		}
		

		public void SetListSuggest(string[] list)
		{
			((AutoCompleteTextBox)ControlToBind).Values = list;
		}
	}
}
