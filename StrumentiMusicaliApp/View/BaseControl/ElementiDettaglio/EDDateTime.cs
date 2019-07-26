using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
    public partial class EDDateTime : EDBase
    {
        public EDDateTime()
            : base()
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
