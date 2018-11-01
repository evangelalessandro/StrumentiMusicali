using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDDateTime : EDBase
	{
		public EDDateTime()
			:base()
		{
			InitializeComponent();

			ControlToBind = new DateEdit();
			 
			panel1.Controls.Add(ControlToBind);
			ControlToBind.Dock = DockStyle.Fill;
			ControlToBind.BringToFront();
			var item = ((DateEdit)ControlToBind);
		}
	}
}
