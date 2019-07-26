using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl.ElementiDettaglio
{
    public partial class EDCombo : EDBase
    {
        private DevExpress.XtraEditors.LookUpEdit cboClienteID;
        public EDCombo() :
            base()
        {
            InitializeComponent();

            cboClienteID = new DevExpress.XtraEditors.LookUpEdit();

            cboClienteID.Properties.ValueMember = "ID";
            cboClienteID.Properties.DisplayMember = "Descrizione";
            cboClienteID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            cboClienteID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            ControlToBind = cboClienteID;
            panel1.Controls.Add(ControlToBind);
            ControlToBind.Dock = DockStyle.Fill;
            ControlToBind.BringToFront();
        }

        public void SetList(object list)
        {
            cboClienteID.Properties.DataSource = list;
            cboClienteID.Properties.BestFit();

        }
        public LookUpEdit Controllo { get { return cboClienteID; } }

    }
}