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
    public partial class MarcaView : Form
    {
        EDCombo _combo = new EDCombo();
        public MarcaView()
        {
            InitializeComponent();
            this.Font = new Font(this.Font.FontFamily, 24);
            _combo.Controllo.Font = this.Font;
            _combo.Titolo = "Marca";
            _combo.Controllo.Properties.Appearance.Font = this.Font;
            _combo.Controllo.Properties.NullText = "Selezionare un elemento";
          
            _combo.Controllo.EditValueChanged += LookUpEdit1_EditValueChanged;
            using (var uof = new UnitOfWork())
            {
                var list = uof.ArticoliRepository.Find(a => a.Strumento.Marca.Length > 0)
                    .Select(a => a.Strumento.Marca.ToUpper()).Distinct().OrderBy(a => a).ToList();

                _combo.Controllo.Properties.ValueMember = "";
                _combo.Controllo.Properties.DisplayMember = "";
                //_combo.SetList(list.Select(a => new { ID = a, Descrizione = a }).ToList());
                _combo.Controllo.Properties.DataSource = list.ToArray();

            }
            _combo.Controllo.Properties.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.False;
            _combo.Controllo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.button1.Click += Button1_Click;
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
            if (_combo.Controllo.EditValue!=null)
                Marca = _combo.Controllo.EditValue.ToString();
            this.Text = "Marca seleziona: " + Marca;
        }
        public string Marca { get; set; }
    }
}
