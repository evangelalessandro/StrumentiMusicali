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
        EDTesto _txt = new EDTesto();

        public string CaptionText { get; set; }
        public ListViewCustom(List<string> list, string titolo, string titoloText = "", bool hideListView = false)
        {
            InitializeComponent();

            this.Font = new Font(this.Font.FontFamily, 24);
            _combo.Controllo.Font = this.Font;
            _combo.Titolo = titolo;
            _combo.Controllo.Properties.Appearance.Font = this.Font;
            _combo.Controllo.Properties.NullText = "Selezionare un elemento";


            _combo.Controllo.Properties.ValueMember = "";
            _combo.Controllo.Properties.DisplayMember = "";
            _combo.Controllo.Properties.DataSource = list.ToArray();

            _combo.Controllo.Properties.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.False;
            _combo.Controllo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.button1.Click += Button1_Click;
            this.Text = titolo;
            this.AcceptButton = button1;

            CaptionText = titoloText;

            if (hideListView)
                _combo.Hide();

        }




        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }

        private void MarcaView_Load(object sender, EventArgs e)
        {
            _combo.Dock = DockStyle.Top;
            this.panel1.Controls.Add(_combo);
            _combo.Controllo.EditValueChanged += Controllo_EditValueChanged;

            if (!string.IsNullOrEmpty(CaptionText))
            {
                _txt.Titolo = CaptionText;
                this.panel1.Controls.Add(_txt);
                _txt.Dock = DockStyle.Bottom;
                _txt.ControlToBind.TextChanged += ControlToBind_TextChanged;
            }
        }

        private void ControlToBind_TextChanged(object sender, EventArgs e)
        {
            EnabledButton();
        }

        private void Controllo_EditValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if (_combo.Controllo.EditValue != null)
                SelectedItem = _combo.Controllo.EditValue.ToString();
            this.Text = "Elemento selezionato: " + SelectedItem;
            EnabledButton();
        }

        private void EnabledButton()
        {
            
            if (_combo.Visible && _combo.Controllo.EditValue != null)
                button1.Enabled = true;

            if (!string.IsNullOrEmpty(CaptionText) && _txt.ControlToBind.Text.Length>0)
            {
                button1.Enabled = true;
            }

        }

        public string SelectedItem { get; set; }

        public string SelectedTextProp
        {
            get { return _txt.ControlToBind.Text; }
        }


    }
}
