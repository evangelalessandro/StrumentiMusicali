using System.Drawing;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDBase : UserControl
	{
		public EDBase()
		{
			InitializeComponent();
			this.MaximumSize = new Size(1000, 150);
			this.Load += EDBase_Load;
		}

		private void EDBase_Load(object sender, System.EventArgs e)
		{
			var textSize = TextRenderer.MeasureText(Titolo, label1.Font).Width;
			if (this.Width< textSize || SetMinSize)
			this.Width= textSize +20;

		}
		public bool SetMinSize { get; set; }
		public string Titolo { get { return label1.Text; } set { label1.Text = value; } }

		public virtual void BindProprieta(string nomeProp, object businessObject)
		{
			ControlToBind.Tag = nomeProp;

			Utility.UtilityView.SetDataBind(this, businessObject);
		}

		public Control ControlToBind { get; set; }
		
	}
}