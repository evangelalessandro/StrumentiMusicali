using DevExpress.XtraEditors;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
    public partial class ListViewCustom : Form
    {
        EDCombo _combo = new EDCombo();
        public ListViewCustom(List<string> list, string titolo)
        {
            InitializeComponent();
            
            this.Font = new Font(this.Font.FontFamily, 24);
            _combo.Controllo.Font = this.Font;
            _combo.Titolo = titolo;
            _combo.Controllo.Properties.Appearance.Font = this.Font;
            _combo.Controllo.Properties.NullText = "Selezionare un elemento";

            _combo.Controllo.EditValueChanged += LookUpEdit1_EditValueChanged;

            _combo.Controllo.Properties.ValueMember = "";
            _combo.Controllo.Properties.DisplayMember = "";
            _combo.Controllo.Properties.DataSource = list.ToArray();

            _combo.Controllo.Properties.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.False;
            _combo.Controllo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.button1.Click += Button1_Click;
            this.Text = titolo;
            this.AcceptButton = button1;
        }




        private void LookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //Display lookup editor's current value.
            LookUpEditBase lookupEditor = sender as LookUpEditBase;
            if (lookupEditor == null) return;
            //LabelControl label = labelDictionary[lookupEditor];
            //if (label == null) return;
            //if (lookupEditor.EditValue == null)
            //    label.Text = "Current EditValue: null";
            //else
            //    label.Text = "Current EditValue: " + lookupEditor.EditValue.ToString();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }

        private void MarcaView_Load(object sender, EventArgs e)
        {
            _combo.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(_combo);
            _combo.Controllo.EditValueChanged += Controllo_EditValueChanged;

        }

        private void Controllo_EditValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if (_combo.Controllo.EditValue != null)
                SelectedItem = _combo.Controllo.EditValue.ToString();
            this.Text = "Elemento selezionato: " + SelectedItem;
        }
        public string SelectedItem { get; set; }
    }
}
