using DevExpress.XtraEditors.Controls;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            using (var uof = new UnitOfWork())
            {
                var list = uof.ArticoliRepository.Find(a => a.Strumento.Marca.Length > 0)
                    .Select(a => a.Strumento.Marca.ToUpper()).Distinct().OrderBy(a=>a).ToList();


                _combo.SetList(list.Select(a => new { ID = a, Descrizione = a }).ToList());
            }
            this.button1.Click += Button1_Click;
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
            Marca = _combo.Controllo.EditValue.ToString();
        }
        public string Marca { get; set; }
    }
}
