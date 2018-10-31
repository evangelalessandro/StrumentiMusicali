using StrumentiMusicali.App.CustomComponents;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
	public partial class EDTesto : EDBase
	{
		public EDTesto() :
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