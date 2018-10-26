using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDNumeric : EDBase
	{
		public EDNumeric() :
			base()
		{
			InitializeComponent();
			ControlToBind = new NumericUpDown();

			panel1.Controls.Add(ControlToBind);
			ControlToBind.Dock = DockStyle.Fill;
			ControlToBind.BringToFront();
		}

		public void SetMinMax(int min, int max)
		{
			(ControlToBind as NumericUpDown).Minimum = min;
			(ControlToBind as NumericUpDown).Maximum = max;
		}
	}
}
