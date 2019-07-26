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
            (ControlToBind as NumericUpDown).TextAlign = HorizontalAlignment.Right;
            panel1.Controls.Add(ControlToBind);
            ControlToBind.Dock = DockStyle.Fill;
            ControlToBind.BringToFront();
            var item = ((NumericUpDown)ControlToBind);
            item.Validating += EDNumeric_Validating;
            item.GotFocus += Item_GotFocus;
        }

        private void Item_GotFocus(object sender, System.EventArgs e)
        {
            var item = ((NumericUpDown)ControlToBind);

            item.Select(0, item.Text.Length);
        }

        private void EDNumeric_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((NumericUpDown)ControlToBind).Value = ((NumericUpDown)ControlToBind).Value;
        }

        public void SetMinMax(int min, int max, int decimalPlace)
        {
            (ControlToBind as NumericUpDown).Minimum = min;
            (ControlToBind as NumericUpDown).Maximum = max;
            (ControlToBind as NumericUpDown).DecimalPlaces = decimalPlace;
        }
    }
}