using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDCheckBok : EDBase
	{
		public EDCheckBok()
			:base()
		{
			InitializeComponent();

			ControlToBind = new CheckBox();
			 
			panel1.Controls.Add(ControlToBind);
			ControlToBind.Dock = DockStyle.Fill;
			ControlToBind.BringToFront();
			var item = ((CheckBox)ControlToBind);
		}
	}
}
