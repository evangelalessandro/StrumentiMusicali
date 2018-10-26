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
	public  partial  class  EDBase : UserControl
	{
		public EDBase()
		{
			InitializeComponent();
			this.MaximumSize=new Size(1000, 30);
		}
		public string Titolo { get { return label1.Text ; } set { label1.Text=value; } }

		public virtual void BindProprieta(string nomeProp, object businessObject)
		{
			 
			ControlToBind.Tag = nomeProp;
			 
			Utility.UtilityView.SetDataBind(this, businessObject);
		}

		public Control ControlToBind { get; set; }

	}
}
