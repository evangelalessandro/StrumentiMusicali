using StrumentiMusicali.Library.Core;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDBase : UserControl
	{
		public EDBase()
		{
			InitializeComponent();
			this.MaximumSize = new Size(1000, 1000);
			this.Load += EDBase_Load;
			this.Paint += SettingBaseView_Paint;
			this.BackColor = Color.Transparent;
		}
		private void SettingBaseView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			var graphics = e.Graphics;
			var gradient_rectangle = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
			System.Drawing.Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(gradient_rectangle, System.Drawing.Color.AliceBlue,
				System.Drawing.Color.LightBlue, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
			graphics.FillRectangle(b, gradient_rectangle);
		}

		private void EDBase_Load(object sender, System.EventArgs e)
		{
			var textSize = TextRenderer.MeasureText(Titolo, label1.Font).Width;
			if (this.Width< textSize || SetMinSize)
				this.Width= textSize +20;

		}
		public bool SetMinSize { get; set; }
		public string Titolo { get { return label1.Text; } set { label1.Text = value; } }

		public virtual void BindProprieta(CustomUIViewAttribute attribute,
			string nomeProp, object businessObject
			)
		{
			ControlToBind.Tag = nomeProp;

			Utility.UtilityView.SetDataBind(this, attribute, businessObject);
		}

		public Control ControlToBind { get; set; }
		
	}
}